import { Grid, Button } from "@material-ui/core";
import React from "react";

import { getSelectableButtonStyle } from "./StyleHelper";

type TafelsSelectorProps = {
  selectTafel: (b: number) => void;
  isSelected: (b: number) => boolean;
};

export function TafelsSelector({
  selectTafel,
  isSelected,
}: TafelsSelectorProps) {
  return (
    <Grid
      container
      justifyContent="center"
      alignItems="center"
      style={{
        marginTop: "1em",
      }}
    >
      <Grid item style={{ width: "20em", backgroundColor: "#f0f0f0" }}>
        <Grid container justifyContent="center" alignItems="center">
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(1)}
              style={getSelectableButtonStyle(() => isSelected(1))}
            >
              1
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(2)}
              style={getSelectableButtonStyle(() => isSelected(2))}
            >
              2
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(3)}
              style={getSelectableButtonStyle(() => isSelected(3))}
            >
              3
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(4)}
              style={getSelectableButtonStyle(() => isSelected(4))}
            >
              4
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(5)}
              style={getSelectableButtonStyle(() => isSelected(5))}
            >
              5
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(6)}
              style={getSelectableButtonStyle(() => isSelected(6))}
            >
              6
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(7)}
              style={getSelectableButtonStyle(() => isSelected(7))}
            >
              7
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(8)}
              style={getSelectableButtonStyle(() => isSelected(8))}
            >
              8
            </Button>
          </Grid>
          <Grid item xs={4}>
            <Button
              onClick={() => selectTafel(9)}
              style={getSelectableButtonStyle(() => isSelected(9))}
            >
              9
            </Button>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
}
