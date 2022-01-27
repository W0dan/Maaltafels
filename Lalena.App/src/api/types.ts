import { components } from "./types.generated";
import { clientFor } from "./api.generic";
import { paths } from "./types.generated";

type Namespaces = "Microsoft.AspNetCore";
type ReplaceAll<
  From extends string,
  To extends string,
  Str extends string
> = Str extends `${infer Pre}${From}${infer Post}`
  ? `${Pre}${To}${ReplaceAll<From, To, Post>}`
  : Str;
type RemovePlusses<T extends string> = ReplaceAll<"+", "", T>;
type RemoveDots<T extends string> = ReplaceAll<".", "", T>;

type ExportedTypes = {
  [FullName in keyof components["schemas"] as FullName extends `${Namespaces}.${infer ShortName}`
    ? RemoveDots<RemovePlusses<ShortName>>
    : RemoveDots<RemovePlusses<FullName>>]: components["schemas"][FullName];
};

export function GetClient() {
  return clientFor<paths>()("https://192.168.1.137:5001", "application/json");
}

export type GetOefeningenResponse = ExportedTypes["GetOefeningenResponse"];
export type Oefening = ExportedTypes["Oefening"];
