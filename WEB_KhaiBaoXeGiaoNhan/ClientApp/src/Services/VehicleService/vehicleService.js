import * as ApiCaller from '../../Libs/httpRequests';

const prefix = 'vehicle';//tên controller, thay đổi theo từng controllẻ khác nhau

// Lấy danh sách xe sở hữu
export function GetVehicleList(pageSize, pageNumber) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+"/getlistvehicle?pageSize="+pageSize+"&pageNumber="+pageNumber+"&vehiclenumber=all",true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tìm biển số xe đã có trả về model xe
export function SearchVehicleModel(searchText) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+"/getlistvehicle?pageSize=1000&pageNumber=1&vehiclenumber="+searchText,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tìm biển số xe đã có trả về bsx
export function SearchBSX(searchText, type) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`${prefix}/find?vehiclenumber=${searchText}&type=${type}`,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Cập nhật thông tin xe
export function UpdateVehicle(vehicleId, driverId, romoocId) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`${prefix}/update?vehicleId=${vehicleId}&driverId=${driverId}&romoocId=${romoocId}`,null,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Thêm mới 1 xe
export function AddVehicle(body) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix+"/create",body,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Xoá xe đã đăng ký
export function RemoveVehicle(vehicleId) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+"/delete?id="+vehicleId,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};