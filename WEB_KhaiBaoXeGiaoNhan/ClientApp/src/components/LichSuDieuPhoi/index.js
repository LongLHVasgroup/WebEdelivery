import React, { useEffect, useState } from "react";
import TextNumber from "../Common/TextNumber";
import HeaderPage from "../Common/HeaderPage";
import { Table, message, Space, Button } from "antd";
import { GetReportLichSuDieuPhoi } from "../../Services/ReportService";
import FormSearchFromTo from "../Common/FormSearchFromTo";
import './index.less'
import moment from "moment";
import { ChangeDieuPhoiIsDone } from "../../Services/DieuPhoi/DieuPhoiService";

const LichSuDieuPhoi = (props) => {

  const [dateFrom, setDateFrom] = useState(moment().startOf("month"))
  const [dateTo, setDateTo] = useState(moment().endOf("month"))

  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isShowUpdate, setIsShowUpdate] = useState(false);

  const renderDate = (createdTime) => {
    if (createdTime === null)
      return;
    const date = moment(createdTime).format('DD/MM/YYYY')
    console.log(date)
    return date;
  }
  const column = [
    {
      title: "Ngày điều phối",
      dataIndex: "createdTime",
      key: "createdTime",
      render: (text) => <>{renderDate(text)}</>
    },
    {
      title: "Dịch vụ vận chuyển",
      dataIndex: "dvvc",
      key: "dvvc",
    },
    {
      title: "Nhà cung cấp",
      dataIndex: "ncc",
      key: "ncc",
      // width: '6%',
    },

    {
      title: "Số đơn hàng",
      dataIndex: "orderNumber",
      key: "orderNumber",
    },
    {
      title: "Số bill",
      dataIndex: "billNumber",
      key: "billNumber",
      // width: '6%',
    },
    {
      title: "Tàu vc",
      dataIndex: "shipNumber",
      key: "shipNumber",
      // width: '6%',
    },
    {
      title: "TL điều phối",
      dataIndex: "soLuong",
      key: "soLuong",
      render: (text) => <TextNumber value={text} />,
      align: "right",
    },
    {
      title: "Số cont",
      dataIndex: "soLuongCont",
      key: "soLuongCont",
      render: (text) => <TextNumber value={text} />,
      align: "right",
    },

    {
      title: "Cung đường",
      dataIndex: "cungDuongName",
      key: "cungDuongName",
    },

    {
      title: "Trạng thái",
      dataIndex: "action",
      key: "action",
      align: "right",
      render: (text, record) => (
        <>
          {record.isDone ? (
            <Space size="middle">

              <a
                key="showDone"
                onClick={() => {
                  update(record.mappingID);
                }}
              >
                <Button type="primary" size="small">Done</Button>
              </a>
            </Space>
          ) : (
            <Space>
              <a
                key="setDone"
                onClick={() => {
                  update(record.mappingID);
                }}
              >
                <Button size="small">Chưa xong</Button>
              </a>
            </Space>
          )}
        </>
      ),
      // width: '10%',
    },
  ];

  const getData = async (from, to) => {
    setIsLoading(true);
    await GetReportLichSuDieuPhoi(from, to).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setData(objRespone.item);
      } else {
        message.error(objRespone.err.msgString);
      }
      setIsLoading(false);
    });
  };

  const update = async (id) => {
    setIsLoading(true);
    await ChangeDieuPhoiIsDone(id).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        message.success(objRespone.err.msgString);
      } else {
        message.error(objRespone.err.msgString);
      }
      setIsLoading(false);
      getData(dateFrom.format('YYYY-MM-DD'), dateTo.format('YYYY-MM-DD'));
    });
  };

  const onHandleSearch = (formValue) => {
    setDateFrom(formValue.fromDate)
    setDateTo(formValue.toDate)
    getData(dateFrom.format('YYYY-MM-DD'), dateTo.format('YYYY-MM-DD'));
    console.log(formValue)
  }


  useEffect(() => {
    getData(dateFrom.format('YYYY-MM-DD'), dateTo.format('YYYY-MM-DD'));
  }, []);
  return (
    <React.Fragment>
      <HeaderPage
        title="Lịch sử điều phối"
        description="Danh sách chi tiết những đơn hàng đã được điều phối cho dịch vụ vận chuyển"
      />
      <FormSearchFromTo
        from={dateFrom}
        to={dateTo}
        onFinish={onHandleSearch}>

      </FormSearchFromTo>
      <Table
        className="my-table"
        columns={column}
        dataSource={data}
        loading={isLoading}
        pagination={{ showSizeChanger: true }}
      ></Table>
    </React.Fragment>
  );
};

export default LichSuDieuPhoi;
