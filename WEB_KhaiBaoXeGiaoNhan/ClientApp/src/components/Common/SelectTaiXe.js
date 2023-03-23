import React, { useEffect, useState } from "react";
import { Select, Button } from "antd";
import { SearchDriver } from "../../Services/DriverService/driverService";
const { Option } = Select;
const SelectTaiXe = (props, { value, onChange }) => {
  const [selectedTaiXe, setSelectedTaiXe] = useState(value);
  const [listTaiXe, setListTaiXe] = useState(props.initTaiXe || []);
  const [textSearch, setTextSearch] = useState("");

  useEffect(() => {
    console.log(selectedTaiXe);
    const identifier = setTimeout(() => {
      
      if (textSearch.length >= 3) callSearchTaiXe();
    }, 500);

    return () => {
      clearTimeout(identifier);
    };
  }, [textSearch]); // gọi api khi thay đổi giá trị của textSearch

  // Tìm kiếm Tên tài xế
  const onSearchHandler = (searchValue) => {
    setTextSearch(searchValue);
  };
  // const onChangeHandler = (value) => {
  //   setSelectedTaiXe(value.key);
  //   if (onChange) onChange(value.key);
  // };
  const callSearchTaiXe = () => {
    SearchDriver(textSearch).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setListTaiXe(objRespone.data);
      } else {
        setListTaiXe([]);
      }
    });
  };

  const optionsTaiXe = listTaiXe.map((item) => (
    <Option key={item.driverId}>
      {item.driverName} - {item.driverCardNo}
    </Option>
  ));

  return (
    <Select
      showSearch
      style={props.style}
      inputValue={selectedTaiXe}
      value={selectedTaiXe}
      showArrow={false}
      filterOption={false}
      onSearch={onSearchHandler}
      onChange={props.onChangeTaiXe}
      notFoundContent={
        <Button onClick={props.showCreateDriver}>Tạo mới</Button>
      }
    >
      {optionsTaiXe}
    </Select>
  );
};

export default SelectTaiXe;
