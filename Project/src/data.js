async function fetchData() {
    const response = await fetch('http://localhost:8080/api/product');

    if (!response.ok) {
        throw new Error('Network response was not ok');
    }

    return response.json();
}

export default fetchData;
