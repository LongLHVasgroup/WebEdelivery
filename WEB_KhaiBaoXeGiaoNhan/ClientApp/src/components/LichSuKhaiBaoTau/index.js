import React, { useEffect, useState } from "react";
import TextNumber from "../Common/TextNumber";
import HeaderPage from "../Common/HeaderPage";
import { Table, message, Input, Form, Row, Col, Button, Typography, Space, Modal } from "antd";
import { GetReportLichSuDieuPhoi } from "../../Services/ReportService";
import FormSearchFromTo from "../Common/FormSearchFromTo";
import { WarningOutlined } from "@ant-design/icons";


import './index.less'
import moment from "moment";
import { DeleteXeKhaiBaoTheoTau, GetDSXeGiaoTheoTau } from "../../Services/KhaiBaoService/KhaiBaoTheoTau";
const { Text } = Typography;
const { confirm } = Modal;

const layoutSearch = {
  labelCol: {
    span: 9,
  },
  wrapperCol: {
    span: 15,
  },
};

const LichSuKhaiBaoTheoTau = (props) => {

  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  const column = [
    {
      title: "Ngày tạo",
      dataIndex: "createdAt",
      key: "createdAt",
      render: (text) => moment(text).format("DD/MM/YYYY"),

    },
    {
      title: "Tên tàu",
      dataIndex: "shipNumber",
      key: "shipNumber",
      align: "right",
    },
    // {
    //   title: "Bill",
    //   dataIndex: "soLuong",
    //   key: "soLuong",
    //   align: "right",
    // },
    {
      title: "Mã đơn hàng",
      dataIndex: "poNumber",
      key: "poNumber",
    },
    {
      title: "Tên mặt hàng",
      dataIndex: "productName",
      key: "productName",
    },
    {
      title: "Biển số xe",
      dataIndex: "vehicleNumber",
      key: "vehicleNumber",
      render: (text, row) => <>{text} - {row.romooc}</>

    },
    {
      title: "Tài xế ca 1",
      dataIndex: "driverName1",
      key: "driverName1",
      render: (text, row) => (
        <p style={{ display: "inline" }}>
          {text}
          <br />
          {row.driverIdCard1}
        </p>
      ),
    },
    {
      title: "Tài xế ca 2",
      dataIndex: "driverName2",
      key: "driverName2",
      render: (text, row) => (
        <p style={{ display: "inline" }}>
          {text}
          <br />
          {row.driverIdCard2}
        </p>
      ),
    },
    {
      title: "Thay đổi",
      dataIndex: "action",
      key: "action",
      align: "right",
      render: (text, record) => (
        <>
          <Space size="middle">
            <a
              key="delete"
              onClick={() => {
                showConfirmDelete(record);
              }}
            >
              <Button size="small" danger>
                Xoá
              </Button>
            </a>
          </Space>
        </>
      ),
      // width: '10%',
    },
  ];


  const showConfirmDelete = (record) => {
    var that = this;
    confirm({
      title: (
        <p>
          Xóa khai báo xe{" "}
          <Text strong> {record.vehicleNumber}</Text>
        </p>
      ),
      icon: <WarningOutlined />,
      content: "",
      // okType: 'default',
      okText: "Huỷ",
      cancelText: "Xoá",
      keyboard: false,
      onOk() {
        console.log("Cancel");
      },
      onCancel() {
        console.log(record)
        // Gọi APi xác nhận xoá Xe
        DeleteXeKhaiBaoTheoTau(record.id).then((objRespone) => {
          if (objRespone.isSuccess === true) {
            message.success(objRespone.err.msgString);
            getData(
              '', ''
            );
          } else {
            message.error(objRespone.err.msgString);
          }
        });
      },
    });
  }

  const getData = async (orderNumber, shipNumber) => {
    setIsLoading(true);
    await GetDSXeGiaoTheoTau(orderNumber, shipNumber, '').then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setData(objRespone.data);
      } else {
        message.error(objRespone.err.msgString);
      }
      setIsLoading(false);
    });
  };

  const onFinish = (formValue) => {
    const orderNumber = formValue.orderNumber !== undefined ? formValue.orderNumber : ''
    const shipNumber = formValue.shipNumber !== undefined ? formValue.shipNumber : ''

    getData(orderNumber, shipNumber);
    console.log(formValue)
  }

  useEffect(() => {
    getData('', '');
  }, []);

  return (
    <React.Fragment>
      <HeaderPage
        title="Danh sách xe đã khai báo theo tàu"
        description=""
      />
      {/* <FormSearchFromTo
        from={dateFrom}
        to={dateTo}
        onFinish={onHandleSearch}>

      </FormSearchFromTo> */}
      <Form
        name="search_from"
        // className={classes['form-search']}
        {...layoutSearch}
        // initialValues={{ fromDate: props.from, toDate: props.to }}
        onFinish={onFinish}
      >
        <Row gutter={24} >
          <Col span={6}
          >
            <Form.Item
              labelAlign='left'
              name='orderNumber'
              label='Mã đơn hàng'
            // rules={[{ required: true, message: 'Chưa chọn ngày' }]}

            >
              <Input placeholder="Nhập mã đơn hàng" />

              {/* <DatePicker className={classes.datePicker} locale={locale} placeholder='' format={dateFormat} /> */}
            </Form.Item>
          </Col>
          <Col span={6}
          // className={classes.column}
          >
            <Form.Item
              // className={classes['form-item']}
              labelAlign='left'
              name='shipNumber'
              label='Tên tàu'
            // rules={[{ required: true, message: 'Chưa chọn ngày' }]}

            >
              {/* <DatePicker className={classes.datePicker} locale={locale} placeholder='' format={dateFormat} /> */}
              <Input placeholder="Tên tàu" />
            </Form.Item>
          </Col>
          <Col span={6}  >
            <Button type="primary" htmlType="submit"> Tìm kiếm </Button>
          </Col>

        </Row>
      </Form>

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

export default LichSuKhaiBaoTheoTau;
