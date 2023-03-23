import * as ApiCaller from "../../Libs/httpRequests";

const prefix = "cungDuong"; //tên controller, thay đổi theo từng controllẻ khác nhau

//trả về json object giống như bên postman gọi ra
export function GetCungDuongList(compannyCode) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(`${prefix}?CompanyCode=${compannyCode}`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

//trả về json object giống như bên postman gọi ra
export function GetCungDuongByUser() {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(`${prefix}/list`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}
