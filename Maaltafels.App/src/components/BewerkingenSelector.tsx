import { Button, Grid } from "@material-ui/core";
import { getSelectableButtonStyle } from "./StyleHelper";

type BewerkingenSelectorProps = {
  selectBewerking: (b: "x" | ":") => void;
  isSelected: (b: "x" | ":") => boolean;
};

export function BewerkingenSelector({
  selectBewerking,
  isSelected,
}: BewerkingenSelectorProps) {
  return (
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
          width: "20em",
          backgroundColor: "#f0f0f0",
          textAlign: "center",
        }}
      >
        <Button
          onClick={() => selectBewerking("x")}
          style={getSelectableButtonStyle(() => isSelected("x"))}
        >
          x
        </Button>
        <Button
          onClick={() => selectBewerking(":")}
          style={getSelectableButtonStyle(() => isSelected(":"))}
        >
          :
        </Button>
      </Grid>
    </Grid>
  );
}
