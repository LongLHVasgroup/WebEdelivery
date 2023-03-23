import * as ApiCaller from "../../Libs/httpRequests";

const prefix = "vehicleservices"; //tên controller, thay đổi theo từng controllẻ khác nhau

// Lưu thông tin điều phối po
export function DieuPhoiSave(body) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpPost(`${prefix}/map`, body, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

// Lấy chi tiết điều phối theo poNumber
export function GetDieuPhoiInfoByPONumber(poNumber) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(`mapping/find?PoNumber=${poNumber}`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

// Lấy danh sách NCC có po đang active
export function GetNCC2DieuPhoi() {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet("providers/find2dieuphoi", true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

// Laasy
export function GetDieuPhoiByPoNumber(poNumber) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(`mapping/get-info?PoNumber=${poNumber}`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

// Cập nhật trạng thái điều phối
export function ChangeDieuPhoiIsDone(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`mapping/${id}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
