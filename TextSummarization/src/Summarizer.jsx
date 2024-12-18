import  { useState } from 'react';

const Summarizer = () => {
  const [inputText, setInputText] = useState('');
  const [summary, setSummary] = useState('');
  const [minLength, setMinLength] = useState(10);  
  const [maxLength, setMaxLength] = useState(30); 
  const [isLoading, setIsLoading] = useState(false); 


  const handleTextChange = (e) => {
    setInputText(e.target.value);
  };

  const handleMinLengthChange = (e) => {
    setMinLength(e.target.value);
  };

  const handleMaxLengthChange = (e) => {
    setMaxLength(e.target.value);
  };

  const handleSummarize = async () => {
    setSummary("");
    try {
      setIsLoading(true);
      const response = await fetch(`https://localhost:7168/api/Summarization?minLenght=${minLength}&maxLenght=${maxLength}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(inputText),
      });

      const data = await response.json();
      if (data && data.length > 0) {
        setSummary(data[0].summary_text);  
      }
      setIsLoading(false);
    } catch (error) {
      console.error('Error during summarization:', error);
    }
  };

  return (
    <div>
            <h1>Text Summarization</h1>
    <div className="Container">
      <div className="ContainerInput">
      <textarea
        value={inputText}
        onChange={handleTextChange}
        placeholder="Enter text to summarize"
        rows="30"
        cols="70"
      />
      <div>
        <label>
          Min Length: 
          <input 
            className="num"
            type="number" 
            value={minLength}
            onChange={handleMinLengthChange} 
            min="1" 
          />
        </label>
        <label>
          Max Length: 
          <input 
            className="num"
            type="number" 
            value={maxLength}
            onChange={handleMaxLengthChange} 
            min="1" 
          />
        </label>
      </div>
      {!isLoading &&<button onClick={handleSummarize}>Summarize</button>}
      </div>
      

      <div>
      <textarea
        value={summary}
        onChange={handleTextChange}
        placeholder="summarized text ..."
        rows="30"
        cols="70"
      />
    </div>

  </div>
</div>
  );
};

export default Summarizer;
