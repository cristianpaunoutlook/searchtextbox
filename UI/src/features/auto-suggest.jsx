import axios from 'axios';
import { useState } from 'react';

import { Products } from '../api/products';

import './auto-suggest.css';

export default function AutoSuggest()
{
    const [filtredListToDisplay, setfiltredListToDisplay] = useState([]);
    const [selectedValue, setSelectedValue] = useState("");
    let cancelTokenSource;

    const displayResult = e => {
        let valueToSearch = e.target.value;
        setSelectedValue(valueToSearch);
        if(valueToSearch.length >= 3){
            //Check if there are any previous pending requests
            if (typeof cancelTokenSource != typeof undefined) {
                cancelTokenSource.cancel("Operation canceled due to new request.")
            }
            cancelTokenSource = axios.CancelToken.source();
            Products.list(valueToSearch, cancelTokenSource).then(response => {
                setfiltredListToDisplay(response);
            }).catch(_ => {
                console.log("error getting data...");
            });
        }
        else{
            setfiltredListToDisplay([]);
        }
    }

    const selectResult = value => {
        setSelectedValue(value.name);
        setfiltredListToDisplay([]);
        Products.incrementWeight(value.key);
    }

    const renderSearchResult = () => {
        return (
            <ul>
                {filtredListToDisplay.map(info =>
                    <li key={info.key} onClick={()=>selectResult(info)}>{info.name}</li>
                )}
            </ul>
            );
    }

    return(
        <div className="auto-suggest">
            <input value={selectedValue} type="text" onChange={displayResult}/>
            {renderSearchResult()}
        </div>
    )
}

