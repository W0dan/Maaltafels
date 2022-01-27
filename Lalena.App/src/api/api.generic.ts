type AcceptedReponseTypes = number | "default";

type ValueOf<T> = T[keyof T];

type ContentResponse = { content: { [key: string]: any } };
type ContentTypesOfContentResponse<TContentResponse> =
  TContentResponse extends ContentResponse
    ? keyof TContentResponse["content"]
    : undefined;
type AllContentTypes<TResponses> = ValueOf<{
  [key in keyof TResponses &
    AcceptedReponseTypes]: TResponses[key] extends never
    ? undefined
    : ContentTypesOfContentResponse<TResponses[key]>;
}>;

type ResponseForContent<
  TContentType extends string,
  TContent extends { [contentType: string]: any }
> = TContent[TContentType];
type ResponseFor<
  TContentType extends string,
  Response
> = Response extends ContentResponse
  ? ResponseForContent<TContentType, Response["content"]>
  : Response;

// type KeysOfType<T, Type> = {
//   [K in keyof T]: T[K] extends Type ? K : never;
// }[keyof T];

// type OmitType<T, Removed> = Omit<T, KeysOfType<T, Removed>>;
// type PickType<T, Excepted> = Pick<T, KeysOfType<T, Excepted>>;

// type RequireType<T, Required> = {
//   [Key in KeysOfType<T, Required>]-?: T[Key];
// } & T;

type AsUnion<T> = ValueOf<{
  [K in keyof T]: { type: K; value: T[K] };
}>;

type MissingHeaders<RequiredHeaders, DefaultHeaders> = Exclude<
  keyof RequiredHeaders,
  DefaultHeaders
>;
type AllRequiredHeadersPresent<RequiredHeaders, DefaultHeaders> =
  MissingHeaders<RequiredHeaders, DefaultHeaders> extends never ? true : false;

type RequestPathPart<Operation> = Operation extends {
  parameters: { path: infer PathParams };
}
  ? {
      path: PathParams & Record<string, string>;
    }
  : {
      path?: Record<string, string>;
    };

type RequestQueryPart<Operation> = Operation extends {
  parameters: { query: infer QueryParams };
}
  ? {
      query: QueryParams & Record<string, string>;
    }
  : {
      query?: Record<string, string>;
    };

type RequestHeadersPart<Operation, DefaultHeaders> = Operation extends {
  parameters: { header: infer HeaderParams };
}
  ? AllRequiredHeadersPresent<HeaderParams, DefaultHeaders> extends true
    ? {
        header?: Record<string, string>;
      }
    : {
        header: Pick<
          HeaderParams,
          MissingHeaders<HeaderParams, DefaultHeaders>
        > &
          Record<string, string>;
      }
  : {
      header?: Record<string, string>;
    };

type RequestBodyPart<Operation> = Operation extends {
  requestBody: { content: infer BodyParam };
}
  ? {
      body: AsUnion<BodyParam>;
    }
  : {
      body?: { type: string; value: any };
    };

type RequestAcceptedContentPart<
  AcceptedContent,
  DefaultAcceptedContent extends string | undefined
> = AcceptedContent extends undefined
  ? { accepted?: string }
  : DefaultAcceptedContent extends undefined
  ? { accepted: AcceptedContent }
  : Extract<AcceptedContent, DefaultAcceptedContent> extends never
  ? { accepted: AcceptedContent }
  : { accepted?: AcceptedContent };

type OperationToFullRequest<
  Operation,
  AcceptedContent,
  DefaultAcceptedContent extends string | undefined = undefined,
  DefaultHeaders extends string | undefined = undefined
> = RequestAcceptedContentPart<AcceptedContent, DefaultAcceptedContent> &
  RequestBodyPart<Operation> &
  RequestPathPart<Operation> &
  RequestQueryPart<Operation> &
  RequestHeadersPart<Operation, DefaultHeaders>;

export type Client<
  Paths,
  DefaultAcceptContentType extends string | undefined = undefined,
  DefaultHeaders extends string | undefined = undefined
> = {
  [path in keyof Paths]: {
    [method in keyof Paths[path]]: Paths[path][method] extends {
      responses: infer TResponses;
    }
      ? AllContentTypes<
          Paths[path][method]["responses"]
        > extends infer AllContentTypes
        ? <
            TReq extends OperationToFullRequest<
              Paths[path][method],
              AllContentTypes,
              DefaultAcceptContentType,
              DefaultHeaders
            >
          >(
            req: TReq
          ) => Promise<
            AsUnion<{
              [response in keyof TResponses &
                AcceptedReponseTypes]: ResponseFor<
                TReq extends { accepted: string }
                  ? TReq["accepted"]
                  : DefaultAcceptContentType extends string
                  ? DefaultAcceptContentType
                  : AllContentTypes & string,
                TResponses[response]
              >;
            }>
          >
        : never
      : never;
    // Paths[path][method] extends { responses: infer TResponses, parameters?: infer TParameters }
    // ? AllContentTypes<Paths[path][method]["responses"]> extends infer AllContentTypes
    // ? TParameters extends object
    // ? {
    // <TContentType extends AllContentTypes>(contentType: TContentType, params: AllowGenericParameters extends true ? TParameters & GenericRequest : TParameters): AsUnion<{
    // [response in (keyof TResponses) & AcceptedReponses]: ResponseFor<TContentType & string, TResponses[response]>
    // }>
    // (params: AllowGenericParameters extends true ? TParameters & GenericRequest : TParameters): AsUnion<{
    // [response in (keyof TResponses) & AcceptedReponses]: ResponseFor<DefaultContentType extends string ? DefaultContentType : AllContentTypes & string, TResponses[response]>
    // }>
    // }
    // : {
    // <TContentType extends AllContentTypes>(contentType: TContentType): AsUnion<{
    // [response in (keyof TResponses) & AcceptedReponses]: ResponseFor<TContentType & string, TResponses[response]>
    // }>
    // (): AsUnion<{
    // [response in (keyof TResponses) & AcceptedReponses]: ResponseFor<DefaultContentType extends string ? DefaultContentType : AllContentTypes & string, TResponses[response]>
    // }>
    // }
    // : never
    // : never
  };
};

type ClientConfig = {
  root: string;
  defaultAcceptContentType: string | undefined;
  defaultHeaders: {};
};

const pathProxyHandler: ProxyHandler<ClientConfig> = {
  get: (target, prop, _receiver) =>
    typeof prop === "string"
      ? new Proxy({ config: target, path: prop }, methodProxyHandler)
      : undefined,
};

const methodProxyHandler: ProxyHandler<{ config: ClientConfig; path: string }> =
  {
    get: (target, prop, _receiver) =>
      typeof prop === "string"
        ? operationBuilder(target.config, target.path, prop)
        : undefined,
  };

const operationBuilder =
  (config: ClientConfig, path: string, method: string) =>
  (
    request: OperationToFullRequest<{ responses: {} }, undefined, undefined>
  ) => {
    const parameterizedPath = Object.keys(request.path || {}).reduce(
      (path, param) => path.replace(`{${param}}`, request.path![param]),
      path
    );
    const contentHeader = request.body
      ? { "Content-Type": request.body.type }
      : ({} as {});
    const acceptHeader = {
      Accept: request.accepted || config.defaultAcceptContentType || "*/*",
    };

    return fetch(
      config.root +
        parameterizedPath +
        "?" +
        new URLSearchParams(request.query),
      {
        method: method,
        headers: {
          ...(config.defaultHeaders || {}),
          ...(request.header || {}),
          ...contentHeader,
          ...acceptHeader,
        },
        body: request.body ? JSON.stringify(request.body.value) : undefined,
      }
    ).then(async (response) => ({
      type: response.status,
      value: await response.json(),
    }));
  };

export const clientFor = <Paths>() => {
  function configureClient(root: string): Client<Paths, undefined, undefined>;
  function configureClient<DefaultAcceptContentType extends string>(
    root: string,
    defaultAcceptContentType: DefaultAcceptContentType
  ): Client<Paths, DefaultAcceptContentType, undefined>;
  function configureClient<
    DefaultAcceptContentType extends string,
    DefaultHeaders extends {}
  >(
    root: string,
    defaultAcceptContentType: DefaultAcceptContentType,
    defaultHeaders: DefaultHeaders
  ): Client<Paths, DefaultAcceptContentType, keyof DefaultHeaders & string>;
  function configureClient<
    DefaultAcceptContentType extends string | undefined,
    DefaultHeaders extends {} | undefined
  >(
    root: string,
    defaultAcceptContentType: DefaultAcceptContentType = undefined as DefaultAcceptContentType,
    defaultHeaders: DefaultHeaders = undefined as DefaultHeaders
  ): any {
    const config: ClientConfig = {
      root: root,
      defaultAcceptContentType: defaultAcceptContentType,
      defaultHeaders: defaultHeaders || {},
    };

    return new Proxy(config, pathProxyHandler) as unknown as Client<
      Paths,
      DefaultAcceptContentType,
      keyof DefaultHeaders & string
    >;
  }

  return configureClient;
};
