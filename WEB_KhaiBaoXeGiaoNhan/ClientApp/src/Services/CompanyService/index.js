import * as ApiCaller from "../../Libs/httpRequests";

const prefix = "v1/company"; //tên controller, thay đổi theo từng controllẻ khác nhau

//trả về json object giống như bên postman gọi ra
export function GetListCompany() {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(`${prefix}`)
      .then((respone) => {
        return resolve(respone);
      })
      .catch((err) => {
        return reject(err);
      });
  });
}
