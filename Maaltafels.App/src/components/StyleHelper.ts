import { CSSProperties } from "react";

export const selectedBgColor = "#808080";
export const selectedFontColor = "#FFFFFF";
export const defaultBgColor = "#d0d0d0";
export const defaultFontColor = "#000000";

export function getSelectableButtonStyle(
  isSelected: () => boolean
): CSSProperties {
  return {
    backgroundColor: isSelected() ? selectedBgColor : defaultBgColor,
    color: isSelected() ? selectedFontColor : defaultFontColor,
    textAlign: "center",
    padding: "1em",
    margin: "1em",
  };
}

export function getButtonStyle(): CSSProperties {
  return {
    backgroundColor: defaultBgColor,
    color: defaultFontColor,
    textAlign: "center",
    padding: "1em",
    margin: "1em",
  };
}
