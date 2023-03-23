import * as cs from "../../Constant/index";
import { DEFAULT_PLANT } from "../../Config/plantConfig";

export function LatestPlant() {
  var plant = localStorage.getItem(cs.LATEST_PLANT) || null;
  if (!plant) {
    plant = DEFAULT_PLANT;
  }
  if (plant === undefined) {
    plant = DEFAULT_PLANT;
  }
  return plant;
}
