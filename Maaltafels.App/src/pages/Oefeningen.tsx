import {useState} from "react";
import {useQuery} from "react-query";
import {GetClient, Oefening} from "../api/types";

import CheckIcon from "@material-ui/icons/CheckCircleOutline";
import WrongIcon from "@material-ui/icons/Error";
import styles from "./Oefeningen.module.css";
import {Button, Grid} from "@material-ui/core";
import {getButtonStyle} from "../components/StyleHelper";

const client = GetClient();

type AnsweredOefening = Oefening & {
    answer: string;
};

async function getOefeningen(bewerkingen: string[], tafels: number[]) {
    const result = await client["/Oefeningen"].get({
        query: {Bewerkingen: bewerkingen.join(","), Tafels: tafels.join(",")},
    });

    switch (result.type) {
        case 200:
            return result.value;
    }
}

type OefeningenProps = {
    bewerkingen: ("x" | ":")[];
    tafels: number[];
    onFinished: () => void;
};

const Resultaat = (
    oefeningenCount: number,
    errors: AnsweredOefening[],
    finished: () => void
) => (
    <Grid
        container
        className={styles.oefeningResultaat}
        style={{
            alignContent: "center",
            verticalAlign: "middle",
            textAlign: "center",
            height: "80vh",
        }}
    >
        <Grid item xs={12} style={{marginBottom: "20px"}}>
            Resultaat = {oefeningenCount - errors.length} / {oefeningenCount}
        </Grid>
        {errors.map((e) => (
            <Grid item xs={12} key={e.opgave}>
                {e.opgave}
                {e.resultaat} Ingevuld: {e.answer}
            </Grid>
        ))}
        <Grid item xs={12}>
            <Button style={getButtonStyle()} onClick={finished}>
                Klaar !
            </Button>
        </Grid>
    </Grid>
);

export function Oefeningen({
                               bewerkingen,
                               tafels,
                               onFinished,
                           }: OefeningenProps) {
    const {data, isLoading} = useQuery(
        ["getOefeningen", bewerkingen, tafels],
        () => getOefeningen(bewerkingen, tafels)
    );

    const [currentOefeningIndex, setCurrentOefeningIndex] = useState(0);
    const [oefeningen, setOefeningen] = useState<Oefening[]>();
    const [answer, setAnswer] = useState<string>();
    const [wrongAnswer, setWrongAnswer] = useState<boolean>();
    const [errors, setErrors] = useState<AnsweredOefening[]>([]);

    if (isLoading) return null;

    if (!oefeningen) {
        setOefeningen(data?.oefeningen ?? []);
        return null;
    }
    const oefeningenCount = oefeningen.length;

    if (oefeningenCount < 0) return <>Geen oefeningen gevonden</>;

    if (currentOefeningIndex >= oefeningenCount)
        return Resultaat(oefeningenCount, errors, onFinished);

    const previousOefening =
        currentOefeningIndex > 0 ? oefeningen[currentOefeningIndex - 1] : null;
    const currentOefening = oefeningen[currentOefeningIndex];
    const nextOefening =
        currentOefeningIndex < oefeningenCount - 1
            ? oefeningen[currentOefeningIndex + 1]
            : null;

    function checkOefening() {
        if (answer) {
            if (parseInt(answer) === currentOefening.resultaat) {
                setCurrentOefeningIndex(currentOefeningIndex + 1);
                setWrongAnswer(false);
            } else {
                if (!wrongAnswer) {
                    setErrors([
                        ...errors,
                        {
                            opgave: currentOefening.opgave,
                            resultaat: currentOefening.resultaat,
                            answer: answer,
                        },
                    ]);
                    setWrongAnswer(true);
                }
            }

            setAnswer(undefined);
        }
    }

    const percentDone = currentOefeningIndex / oefeningenCount * 100;
    const percentTodo = 100 - percentDone;

    return (
        <>
            <Grid
                container
                className={styles.theContainer}
                style={{
                    alignContent: "center",
                    verticalAlign: "middle",
                    textAlign: "center",
                    paddingLeft: "2em",
                    paddingRight: "2em",
                }}
            >
                <Grid item xs={12} style={{margin: "0.5em"}}>
                    Gekozen bewerkingen:
                    {" " +
                        bewerkingen
                            .map((t) => `${t}`)
                            .reduce((result, current) =>
                                !result ? current : `${result}, ${current}`
                            )}
                </Grid>
                <Grid item xs={12} style={{margin: "0.5em"}}>
                    Gekozen tafels:
                    {" " +
                        tafels
                            .map((t) => `${t}`)
                            .reduce((result, current) =>
                                !result ? current : `${result}, ${current}`
                            )}
                </Grid>
            </Grid>
            <Grid
                container
                className={styles.theContainer}
                style={{
                    alignContent: "center",
                    verticalAlign: "middle",
                    textAlign: "center",
                    height: "60vh",
                }}
            >
                <Grid item xs={12} className={styles.oefeningPreviousNext}>
                    {!nextOefening ? <></> : <>{nextOefening.opgave}</>}
                </Grid>
                <Grid item xs={3} style={{textAlign: "left"}}>
                    <ul>
                        <li>Oefening {currentOefeningIndex + 1} van {oefeningenCount}</li>
                        <li style={{color: errors.length === 0 ? "green" : "#E00000"}}>Fouten tot
                            hiertoe: {errors.length}</li>
                    </ul>
                </Grid>
                <Grid
                    item
                    xs={3}
                    className={styles.oefeningOpgave}
                    style={{textAlign: "right"}}
                >
                    {currentOefening.opgave}
                </Grid>
                <Grid item xs={1} className={styles.oefeningOpgave}>
                    <input
                        type="text"
                        style={{width: "5em", height: "30px"}}
                        onChange={(e) => setAnswer(e.target.value)}
                        onKeyUp={(e) =>
                            e.code === "NumpadEnter" || e.code === "Enter"
                                ? checkOefening()
                                : null
                        }
                        value={answer ?? ""}
                    />
                </Grid>
                <Grid item xs={5} style={{textAlign: "left", padding: "1em"}}>
                    <CheckIcon className={styles.checkIcon} onClick={checkOefening}/>
                    {!wrongAnswer ? <></> : <WrongIcon style={{color: "#E00000"}}/>}
                </Grid>
                <Grid item xs={12} className={styles.oefeningPreviousNext}>
                    {!previousOefening ? (
                        <></>
                    ) : (
                        <>
                            {previousOefening.opgave} {previousOefening.resultaat}
                        </>
                    )}
                </Grid>
            </Grid>
            <Grid
                container
                className={styles.theContainer}
                style={{
                    alignContent: "center",
                    verticalAlign: "middle",
                    textAlign: "center",
                    paddingLeft: "2em",
                    paddingRight: "2em",
                }}
            >
                <Grid item xs={12} style={{border: "1px solid #808080"}}>
                    <div style={{backgroundColor: "#00CC00", width: `${percentDone}%`, height: "100%", display: "inline-block"}}>&nbsp;<br />&nbsp;</div>
                    <div style={{backgroundColor: "#EEEEEE", width: `${percentTodo}%`, height: "100%", display: "inline-block"}}>&nbsp;<br />&nbsp;</div>    
                </Grid>
            </Grid>
        </>
    );
}
