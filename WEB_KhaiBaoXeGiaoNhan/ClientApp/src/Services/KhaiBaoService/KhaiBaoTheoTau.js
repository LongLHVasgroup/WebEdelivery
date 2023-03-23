import * as ApiCaller from "../../Libs/httpRequests";

const prefix = "RegisterForShip"; //tên controller, thay đổi theo từng controllẻ khác nhau


// Lưu danh sách xe đăng ký giao nhận
export function AddVehicleRegisterForShip(body) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpPost(prefix, body, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}


export function UpdateVehicleRegisterForShip(body) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpPut(prefix, body, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

export function GetDSXeGiaoTheoTau(orderNumber, shipNumber, productCode) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(`${prefix}?orderNumber=${orderNumber}&shipNumber=${shipNumber}&productCode=${productCode}`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}



export function DeleteXeKhaiBaoTheoTau(id) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpDelete(`${prefix}/${id}`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}