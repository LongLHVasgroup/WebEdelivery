import * as ApiCaller from '../../Libs/httpRequests';

const prefix = 'user';//tên controller, thay đổi theo từng controllẻ khác nhau

//trả về json object giống như bên postman gọi ra
export function GetUserInFo(username) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + '/'+username).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Update thông tin user
export function UpdateUserInfo(body) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix + "/info/change",body,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Đổi mật khẩu
export function ChangePassword(body) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix + "/password/change",body,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};