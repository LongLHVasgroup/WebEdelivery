import React from "react";

const TextPercent = (props) => {
  return (
    <>
      {new Intl.NumberFormat("de-DE", {
        style: "percent",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }).format(props.value)}
    </>
  );
};
export default TextPercent;
