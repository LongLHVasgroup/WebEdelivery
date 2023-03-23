import * as ApiCaller from "../../Libs/httpRequests";

const prefix = "orders"; //tên controller, thay đổi theo từng controllẻ khác nhau

//trả về json object giống như bên postman gọi ra
export function GetActiveOrderList(companyCode) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(`${prefix}?companyCode=${companyCode}`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}

// lấy thông tin đơn hàng
// orders/find?ponumber=33459
export function SearchPOByNo(poNumber) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(prefix + `/find?ponumber=${poNumber}`, true)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}
