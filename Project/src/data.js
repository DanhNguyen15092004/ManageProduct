async function fetchData() {
    try {
      const response = await fetch('http://localhost:8080/api/product');
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Fetch error:', error);
      throw error; // Re-throw the error to be handled by the caller if needed
    }   
  }  
  
  export default fetchData;