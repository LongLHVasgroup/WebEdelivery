import * as ApiCaller from "../../Libs/httpRequests";

const prefix = "reports"; //tên controller, thay đổi theo từng controllẻ khác nhau

// https://localhost:5001/api/reports/Web_Chart_Summary?from=2021-04-04&to=2021-05-04
export function GetDataReportChartLine(fromDate, toDate) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(
      prefix + `/Web_Chart_Summary?from=${fromDate}&to=${toDate}`,
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

export function GetDataReportChartBar(fromDate, toDate) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(
      prefix + `/Web_Chart_Summary?from=${fromDate}&to=${toDate}`,
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

export function GetDataReportChart(fromDate, toDate, plant) {
  return new Promise((resolve, reject) => {
    return ApiCaller.httpGet(
      `${prefix}/Web_Chart_Summary?from=${fromDate}&to=${toDate}&plant=${plant}`,
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
