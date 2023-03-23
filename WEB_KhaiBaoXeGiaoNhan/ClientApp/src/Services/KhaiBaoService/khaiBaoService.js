// Tạo mới
import * as ApiCaller from "../../Libs/httpRequests";

const prefix = "VehicleRegister"; //tên controller, thay đổi theo từng controllẻ khác nhau

// Lưu danh sách xe đăng ký giao nhận
export function AddVehicleRegisterPO(body) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpPost(prefix + "/po", body, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

// Lấy danh sách giao nhận để edit
export function GetVehicleRegisterList2Edit(
  startDate,
  endDate,
  orderNumber,
  plant,
  isCustomer
) {
  if (plant === undefined) plant = "all";
  console.log("plant: ", plant);
  if (!isCustomer) {
    if (
      startDate === undefined &&
      endDate === undefined &&
      orderNumber === undefined
    ) {
      return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(
          `${prefix}/find?po=all&isScales=false&plant=${plant}`,
          true
        )
          .then((respone) => {
            return resolve(respone);
          })
          .catch((err) => {
            return reject(err);
          });
      });
    } else if (orderNumber === undefined || orderNumber === "") {
      return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(
          `${prefix}/find?po=all&isScales=false&from=${startDate}&to=${endDate}&plant=${plant}`,
          true
        )
          .then((respone) => {
            return resolve(respone);
          })
          .catch((err) => {
            return reject(err);
          });
      });
    } else {
      return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(
          `${prefix}/find?po=${orderNumber}&isScales=false&from=${startDate}&to=${endDate}&plant=${plant}`,
          true
        )
          .then((respone) => {
            return resolve(respone);
          })
          .catch((err) => {
            return reject(err);
          });
      });
    }
  }
}

// Lấy danh sách giao nhận để view
export function GetVehicleRegisterList2View(
  startDate,
  endDate,
  orderNumber,
  plant,
  isCustomer
) {
  if (plant === undefined) plant = "all";
  if (!isCustomer) {
    if (
      startDate === undefined &&
      endDate === undefined &&
      orderNumber === undefined
    ) {
      return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`${prefix}/find?po=all&isScales=true`, true)
          .then((respone) => {
            return resolve(respone);
          })
          .catch((err) => {
            return reject(err);
          });
      });
    } else if (orderNumber === undefined) {
      return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(
          `${prefix}/find?po=all&isScales=true&from=${startDate}&to=${endDate}&plant=${plant}`,
          true
        )
          .then((respone) => {
            return resolve(respone);
          })
          .catch((err) => {
            return reject(err);
          });
      });
    } else {
      return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(
          `${prefix}/find?po=${orderNumber}&isScales=true&from=${startDate}&to=${endDate}&plant=${plant}`,
          true
        )
          .then((respone) => {
            return resolve(respone);
          })
          .catch((err) => {
            return reject(err);
          });
      });
    }
  }
}

// Xoá đăng ký giao nhận
export function RemoveVehicleRegister(vehicleRegisterId) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpDelete(prefix + "/delete/" + vehicleRegisterId, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}


export function RemoveMultipleVehicleRegister(arrVehicleRegId){
  return new Promise((resolve, reject) => {
    return ApiCaller.httpPost(prefix + "/delete/records",arrVehicleRegId, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}
// Cập nhật khai báo
export function UpdateVehicleRegister(id, body) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpPut(prefix + "/update?id=" + id, body, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

// Active xe đã khai báo
export function ActiveVehicleRegister(id) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpPut(prefix + "/update/active?id=" + id, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}
