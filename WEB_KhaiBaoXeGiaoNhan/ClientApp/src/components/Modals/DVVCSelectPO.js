import React, { useEffect, useState } from "react";
import { Table, Modal, Tooltip } from "antd";
import TextNumber from "../Common/TextNumber";
import moment from "moment";

const DVVCSelectPO = (props) => {
  const [selectedPO, setSelectedPO] = useState(props.selectedPO);
  const [selectedPOKeyArr, setSelectedPOKeyArr] = useState([]);
  //   const []

  // Xóa thông tin mỗi khi có ds po mới
  useEffect(() => {
    setSelectedPO(null);
    setSelectedPOKeyArr([]);
  }, [props.dataSource]);

  const columns = [
    {
      title: "Số bill",
      dataIndex: "billNumber",
      render: (text, row) => (
        <Tooltip title={row.note}>
          <span>{text}</span>
        </Tooltip>
      ),
      width: "16%",
      key: "billNumber",
    },
    {
      title: (
        <>
          Mã số
          <br />
          đơn hàng
        </>
      ),
      dataIndex: "order",
      render: (text, row) => (
        <Tooltip
          title={
            localStorage.getItem("isService") == "true"
              ? row.note
              : row.provider
          }
        >
          <span>{text}</span>
        </Tooltip>
      ),
      // width: '16%'
      key: "order",
    },
    {
      title: (
        <>
          Thời hạn
          <br />
          hiệu lực
        </>
      ),
      dataIndex: "deliveryDate",
      render: (text) => moment(text).format("DD/MM/YYYY"),
      align: "right",
      width: "14%",
      key: "deliveryDate",
    },
    {
      title: "Số lượng, kg",
      children: [
        {
          title: "Đơn hàng",
          dataIndex: "total",
          align: "right",
          render: (text) => <TextNumber value={text} />,
          // width: '20%'
          key: "total",
        },
        {
          title: "Đã khai báo",
          dataIndex: "registered",
          align: "right",
          render: (text) => (
            <>{text === null ? <>&#8210;</> : <TextNumber value={text} />}</>
          ),
          // width: '20%'
          key: "registered",
        },

        // {
        //   title: 'Đã giao',
        //   dataIndex: 'soLuongDaChuyen',
        //   align: 'right',
        //   render: (text) => <>{text === null ? <>&#8210;</> : new Intl.NumberFormat('de-DE').format(text)}</>,
        //   // width: '20%'
        // },
        {
          title: "Còn lại",
          align: "right",
          dataIndex: "conLai",
          render: (text) => <TextNumber value={text} />,
          // width: '20%',
          key: "conLai",
        },
      ],
    },
    {
      title: (
        <>
          Số lượng
          <br />
          cont
        </>
      ),
      align: "right",
      dataIndex: "soLuongCont",
      render: (text) => <TextNumber value={text} />,
      //
      // width: '20%',
      key: "soLuongCont",
    },
    {
      title: (
        <>
          Tên tàu
        </>
      ),
      align: "right",
      dataIndex: "shipNumber",
      //
      // width: '20%',
      key: "shipNumber",
    }
  ];

  const onOkHandler = () => {
    props.onOk(selectedPO);
  };

  return (
    <Modal
      title="Chọn đơn hàng"
      centered
      visible={props.visible}
      onOk={onOkHandler}
      okText="Chọn"
      cancelText="Huỷ"
      onCancel={props.onCancel}
      width={900}
    >
      <Table
        className="my-table-order-list"
        locale={{ emptyText: "Không có dữ liệu" }}
        rowSelection={{
          selectedRowKeys: selectedPOKeyArr,
          type: "radio",
          onChange: (selectedRowKeys, selectedRows) => {
            setSelectedPOKeyArr(selectedRowKeys);
            setSelectedPO(selectedRows[0]);
          },
        }}
        onRow={(record, rowIndex) => {
          return {
            onClick: (event) => {
              setSelectedPOKeyArr([record.key]);
              setSelectedPO(record);
            }, // click row
          };
        }}
        columns={columns}
        dataSource={props.dataSource}
        bordered
        pagination={false}
        loading={props.loading}
      />
    </Modal>
  );
};
export default DVVCSelectPO;
