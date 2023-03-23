import * as ApiCaller from '../../Libs/httpRequests';

const prefix = 'reports';//tên controller, thay đổi theo từng controllẻ khác nhau

// from=2021-04-06&to=2021-04-07
// Lấy danh sách xe sở hữu
export function GetReportGiaoNhan(pageSize, pageNumber, fromDate, toDate, plant) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`${prefix}/reportnhapxuat?from=${fromDate}&to=${toDate}&plant=${plant}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Lấy thông tin vận chuyển của các nhà vận chuyển
export function GetReportVanChuyen(plant) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/reportvanchuyen?plant=${plant}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Lấy thông tin nội địa và nhập khẩu
export function GetReportTrongLuongChung(fromDate, toDate, plant) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/reportnhapkhaunoidia?from=${fromDate}&to=${toDate}&plant=${plant}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tiến độ giao hàng
// https://localhost:5001/api/reports/reporttiendogiaohang?from=2020-04-01&to=2020-05-09
export function GetReportTienDoGiaoHang(pageSize, pageNumber, fromDate, toDate) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/reporttiendogiaohang?from=${fromDate}&to=${toDate}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tổng hợp tiến độ giao đơn hàng
// https://localhost:5001/api/reports/reporttonghop?ponumber=4500033490
export function GetReportChiTietTienDoGiaoHang(ponumber, plant) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/reporttonghop?ponumber=${ponumber}&plant=${plant}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tình hình đơn hàng được điều phối
// https://localhost:5001/api/reports/reporttinhhinhvanchuyen?from=2021-04-04
export function GetReportTinhHinhDonHangDieuPhoi(from, plant) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/reporttinhhinhvanchuyen?from=${from}&plant=${plant}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Lịch sử đơn hàng đã được điều phối
export function GetReportLichSuDieuPhoi(from, to) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/lichsudieuphoi?from=${from}&to=${to}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tình hình đăng ký giao của NCC
export function GetReportTinhHinhDangKy(from,to, plant) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/tinhhinhdangky?from=${from}&to=${to}&plant=${plant}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};