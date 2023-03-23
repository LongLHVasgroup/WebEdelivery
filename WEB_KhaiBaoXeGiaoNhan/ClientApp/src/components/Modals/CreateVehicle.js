import React, { Component, useEffect, useState } from "react";
import {
  Modal,
  Button,
  Input,
  Radio,
  InputNumber,
  Form,
  Select,
  Typography,
  message,
} from "antd";
import { SearchDriver } from "../../Services/DriverService/driverService";
import { WarningOutlined } from "@ant-design/icons";
import {
  AddVehicle,
  SearchBSX,
} from "../../Services/VehicleService/vehicleService";
import { FormatedNumber, WeightParser } from "../Common/TextNumber";
const { Text } = Typography;
const { confirm } = Modal;

const layoutAddVehicle = {
  labelCol: {
    span: 11,
  },
  wrapperCol: {
    span: 13,
  },
};
const { Option } = Select;

function CreateVehicleModal(props) {
  const [taiXeList, setTaiXeList] = useState([]);
  const [romoocList, setRomoocList] = useState([]);
  const [textSearch, setTextSearch] = useState("");
  const [romoocSearch, setRomoocSearch] = useState("");
  const [txtTLChoPhep, setTxtTLChoPhep] = useState(
    "Khối lượng toàn bộ cho phép TGGT (kg)"
  );
  const [isDauKeo, setIsDauKeo] = useState(false);
  const [isLoadingRomooc, setIsLoadingRomooc] = useState(false);

  //useEffect call api search tai xe
  useEffect(() => {
    if (textSearch.length >= 3) {
      const searchTaiXeTimeout = setTimeout(() => {
        fetchTaiXe(textSearch);
      }, 500);

      return () => {
        clearTimeout(searchTaiXeTimeout);
      };
    }
  }, [textSearch]);

  const searchTaixeHandler = (text) => {
    setTextSearch(text);
  };

  useEffect(() => {
    if (romoocSearch.length >= 3) {
      const searchRomoocTimeout = setTimeout(() => {
        fetchRomooc(romoocSearch);
      }, 500);

      return () => {
        clearTimeout(searchRomoocTimeout);
      };
    }
  }, [romoocSearch]);

  const searchRomoocHandler = (text) => {
    setRomoocSearch(text);
  };

  // Option Tai xe
  const optionsTaiXe = taiXeList.map((d) => (
    <Option key={d.driverId}>
      {d.driverName} - {d.driverCardNo}
    </Option>
  ));

  // Tìm kiếm Tài xế
  const fetchTaiXe = (text) => {
    //setIsFetchingTaiXe(true);
    SearchDriver(text).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setTaiXeList(objRespone.data);
      } else {
        setTaiXeList([]);
      }
      //   setIsFetchingTaiXe(false);
    });
  };

  // Tìm kiếm biển số xe
  const fetchRomooc = (text) => {
    setIsLoadingRomooc(true);
    SearchBSX(text, "romooc").then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setRomoocList(objRespone.data);
      } else {
        setRomoocList([]);
      }
      setIsLoadingRomooc(false);
    });
  };

  const optionsRomooc = romoocList.map((d) => (
    <Option key={d.vehicleId} value={d.vehicleId}>
      {d.vehicleNumber}
    </Option>
  ));


  const changeVehicleTypeHandler = (type) => {
    console.log(type.target.value);
    switch (type.target.value) {
      case 1:
      case 3:
        setTxtTLChoPhep("Khối lượng toàn bộ cho phép TGGT (kg)");
        setIsDauKeo(false);
        break;
      case 2:
        setTxtTLChoPhep("Khối lượng kéo theo cho phép (kg)");
        setIsDauKeo(true);
        break;
    }
  };

  // Show xác nhận thêm mới xe
  const showConfirmCreate = (values) => {
    confirm({
      title: "Bạn có chắc đã nhập đúng thông tin",
      icon: <WarningOutlined />,
      content: "Xe khi được tạo mới sẽ không thể chỉnh sửa",
      okText: "Huỷ",
      cancelText: "Lưu",
      keyboard: false,
      onOk() {},
      onCancel() {
        console.log("Cancel");
        // Gọi APi Thêm xe mới

        switch (values.type) {
          case 1:
            values.isDauKeo = false;
            break;
          case 2:
            values.isDauKeo = true;
            break;
          case 3:
            values.isRoMooc = 1;
            break;
        }
        // xóa khoản trắng
        var bsx = values.vehicleNumber.replaceAll(" ", "");
        console.log(bsx);
        values.vehicleNumber = bsx;

        if (values.driverId === undefined)
          values.driverId = "00000000-0000-0000-0000-000000000000"; // giá trị mặc định khi không chọn tài xế
        console.log(values);
        AddVehicle(values).then((objRespone) => {
          if (objRespone.isSuccess === true) {
            message.success(objRespone.err.msgString);
            props.onSuccess();
          } else {
            message.error(objRespone.err.msgString);
          }
        });
      },
    });
  };

  return (
    <Modal
      width={640}
      bodyStyle={{
        padding: "32px 40px 48px",
      }}
      title="Tạo xe mới"
      visible={props.createModalVisible}
      footer={[
        <Button type="primary" form="formAdd" key="submit" htmlType="submit">
          Lưu
        </Button>,
      ]}
      onCancel={() => {
        props.onCancel();
      }}
      destroyOnClose={true}
      width={720}
    >
      <Form
        {...layoutAddVehicle}
        name="vehicle-register"
        id="formAdd"
        initialValues={{ type: 1 }}
        onFinish={showConfirmCreate}
      >
        <Form.Item
          labelAlign="left"
          name="vehicleNumber"
          label="Biển số xe"
          rules={[
            {
              required: true,
              message: "Chưa nhập biển số xe",
            },
            {
              max: 13,
              message: "Quá ký tự cho phép",
            },
          ]}
          normalize={(input) => input.toUpperCase()}
        >
          <Input
            tyle={{
              width: "100%",
            }}
            placeholder ="VD: 51C-999.99 hoặc 51C-9999"
          />
          {/* <Text type="danger"> Ví dụ 51C-999.99 hoặc 51C-9999 KHÔNG được viết liền nhau 51C99999 </Text> */}
        </Form.Item>
        
        <Form.Item
          labelAlign="left"
          name="type"
          label="Loại xe"
          rules={[
            {
              required: true,
              message: "Chưa chọn kiểu xe",
            },
          ]}
        >
          <Radio.Group onChange={changeVehicleTypeHandler}>
            <Radio value={1}>Xe thường</Radio>
            <Radio value={2}>Đầu kéo</Radio>
            <Radio value={3}>Rơ mooc</Radio>
          </Radio.Group>
        </Form.Item>

        <Form.Item
          labelAlign="left"
          name="romoocId"
          label="Romooc kéo theo"
          hidden={!isDauKeo}
        >
          <Select
            showSearch
            defaultActiveFirstOption={false}
            filterOption={false}
            placeholder="Tìm kiếm Romooc"
            onSearch={searchRomoocHandler}
            notFoundContent={<Text></Text>}
            loading = {isLoadingRomooc}
          >
            {optionsRomooc}
          </Select>
        </Form.Item>

        <Form.Item
          className="my-input-number"
          labelAlign="left"
          name="vehicleWeight"
          label="Khối lượng bản thân (kg)"
          rules={[
            {
              type: "number",
              min: 1,
              max: 99999,
              required: true,
              message: "Vui lòng nhập đúng thông tin",
            },
          ]}
        >
          <InputNumber
            className="my-input-number"
            style={{
              width: "100%",
            }}
            formatter={(value) => FormatedNumber(value)}
            parser={WeightParser}
          />
        </Form.Item>
        <Form.Item
          className="my-input-number"
          labelAlign="left"
          name="trongLuongDangKiem"
          label={txtTLChoPhep}
          rules={[
            {
              type: "number",
              min: 1,
              max: 99999,
              required: true,
              message: "Vui lòng nhập đúng thông tin",
            },
          ]}
        >
          <InputNumber
            className="my-input-number"
            style={{
              width: "100%",
            }}
            formatter={(value) => FormatedNumber(value)}
            parser={WeightParser}
          />
        </Form.Item>
        {/* Chọn tài xế theo xe */}
        <Form.Item labelAlign="left" name="driverId" label="Tài xế">
          <Select
            showSearch
            defaultActiveFirstOption={false}
            filterOption={false}
            placeholder="Tìm kiếm tài xế"
            onSearch={searchTaixeHandler}
            notFoundContent={<Text></Text>}
          >
            {optionsTaiXe}
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
}

export default CreateVehicleModal;
