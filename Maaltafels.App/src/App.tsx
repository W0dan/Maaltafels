import { Button, Grid, Typography } from "@material-ui/core";
import { useState } from "react";
import { QueryClient, QueryClientProvider } from "react-query";
import "./App.css";
import { Oefeningen } from "./pages/Oefeningen";
import { BewerkingenSelector } from "./components/BewerkingenSelector";
import { TafelsSelector } from "./components/TafelsSelector";
import { getButtonStyle } from "./components/StyleHelper";

const queryClient = new QueryClient();

function isSelected(array: any[], clickedValue: any) {
  return array.some((b) => b === clickedValue);
}

function App() {
  const [bewerkingen, setBewerkingen] = useState<("x" | ":")[]>([]);
  const [tafels, setTafels] = useState<number[]>([]);
  const [started, setStarted] = useState(false);
  const [error, setError] = useState("");

  function handleBewerkingClick(bewerking: "x" | ":") {
    handleElementClick(bewerkingen, bewerking, setBewerkingen);
  }

  function handleTafelClick(tafel: number) {
    handleElementClick(tafels, tafel, setTafels);
  }

  function handleElementClick(
    array: any[],
    element: any,
    setFn: (e: any) => void
  ) {
    const foundIndex = array.findIndex((b) => b === element);
    if (foundIndex >= 0) {
      console.log(foundIndex);
      array.splice(foundIndex, 1);
      setFn([...array]);
    } else {
      setFn([...array, element]);
    }
  }

  function startOefeningen() {
    if (bewerkingen.length <= 0) {
      setError("Kies x of : of beide");
      return;
    }

    if (tafels.length <= 0) {
      setError("Kies minstens 1 tafel");
      return;
    }

    setError("");
    setStarted(true);
  }

  return (
    <QueryClientProvider client={queryClient}>
      <div className="App">
        <header className="App-header">Maaltafels</header>
        {!started ? (
          <>
            <Grid
              container
              justifyContent="center"
              alignItems="center"
              style={{
                marginTop: "1em",
              }}
            >
              <Grid
                item
                style={{
                  textAlign: "center",
                  width: "20em",
                  backgroundColor: "#f0f0f0",
                }}
              >
                <Typography style={{ color: "red" }}>{error}</Typography>
              </Grid>
            </Grid>

            <BewerkingenSelector
              isSelected={(b) => isSelected(bewerkingen, b)}
              selectBewerking={(b) => handleBewerkingClick(b)}
            />

            <TafelsSelector
              isSelected={(t) => isSelected(tafels, t)}
              selectTafel={(t) => handleTafelClick(t)}
            />

            <Grid
              container
              justifyContent="center"
              alignItems="center"
              style={{
                marginTop: "1em",
              }}
            >
              <Grid
                item
                style={{
                  textAlign: "center",
                  width: "20em",
                  backgroundColor: "#f0f0f0",
                }}
              >
                <Button style={getButtonStyle()} onClick={startOefeningen}>
                  Start oefeningen
                </Button>
              </Grid>
            </Grid>
          </>
        ) : (
          <Oefeningen
            bewerkingen={bewerkingen}
            tafels={tafels}
            onFinished={() => {
              setStarted(false);
            }}
          />
        )}
      </div>
    </QueryClientProvider>
  );
}

export default App;
