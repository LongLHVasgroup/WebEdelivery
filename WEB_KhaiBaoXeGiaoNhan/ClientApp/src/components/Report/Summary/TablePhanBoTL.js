import React from "react";
import { Table } from "antd";
import TextPercent from "../../Common/TextPercent";
import TextNumber from "../../Common/TextNumber";
import classes from './TablePhanBoTL.module.css'
const TablePhanBoTL = (props) => {


    const columns = [
        {
            title: "stt",
            dataIndex: "stt",
            key: "stt",
            align: "left",
            width: 30,
        },
        {
            title: "Tên nhà cung cấp",
            dataIndex: "providerName",
        },
        {
            title: "Trọng lượng đã giao",
            dataIndex: "value",
            align: "right",
            render: (text) => (
                <TextNumber
                    style={{ whiteSpace: "nowrap", margin: "auto" }}
                    value={text}
                />
            ),
            width: 100,
        },
        {
            title: "Tỷ lệ",
            dataIndex: "percent",
            align: "right",
            render: (text) => (
                <TextPercent
                    style={{ whiteSpace: "nowrap", margin: "auto" }}
                    value={text}
                />
            ),
            width: 70,
        },
    ];


    return (
        <Table
            showHeader={false}
            dataSource={props.data}
            pagination={false}
            scroll={{ y: 330 }}
            columns={columns}
            className="table"
            summary={() => (
                <Table.Summary fixed>
                    <Table.Summary.Row
                        fixed
                        postion="bottom"
                        style={{ background: "#96969641" }}
                    >
                        <Table.Summary.Cell index={1} colSpan={2}>
                            Tổng cộng
                        </Table.Summary.Cell>
                        <Table.Summary.Cell
                            align="right"
                            className={classes.weight}
                            index={2}
                            colSpan={1}
                        >
                            <TextNumber
                                value={props.total}
                            />
                        </Table.Summary.Cell>
                        <Table.Summary.Cell
                            index={3}
                            align="right"
                            colSpan={1}
                        >
                            <TextPercent
                                value={props.percent}
                            />
                        </Table.Summary.Cell>
                    </Table.Summary.Row>
                </Table.Summary>
            )}
        />
    )
}
export default TablePhanBoTL;