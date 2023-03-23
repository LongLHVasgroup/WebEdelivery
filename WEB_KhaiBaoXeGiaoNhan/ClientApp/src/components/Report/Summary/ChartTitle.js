import React from 'react'
import classes from './ChartTitle.module.css'

const ChartTitle = (props) => {
    return (
        <h5 className={classes.title}>
            <b>{props.title}</b>
        </h5>
    )
}
export default ChartTitle;