import fetchData from './data.js';

const wrapProd = document.querySelector(".wrapProductItem");

async function addProductStructureToHtml(productAttribute) {
    const { productsName, currentPrice, dateCreate, id } = productAttribute;

    const productElement = document.createElement('li');
    productElement.className = 'ProductItem';

    productElement.innerHTML = `
        <ul id="ProdProperty">
            <li class="prodAttribute">Tên sản phẩm : ${productsName}</li>
            <li class="prodAttribute">Giá tiền : ${currentPrice}</li>
            <li class="prodAttribute">Ngày tạo : ${dateCreate}</li>
            <i class="fa-solid fa-trash-can deleteButton" id="${id}"></i>
        </ul>
    `;

    const deleteButton = productElement.querySelector(".deleteButton");
    deleteButton.addEventListener("click", async (event) => {
        const prodID = event.target.id;
        try {
            const response = await fetch(`http://localhost:8080/api/product/${prodID}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json"
                },
            });

            if (!response.ok) {
                throw new Error("Network response was not ok");
            } else {
                console.log("Xóa thành công");
            }
        } catch (error) {
            console.error('Error:', error);
        }
    });

    wrapProd.appendChild(productElement);
}

(async () => {
    try {
        const data = await fetchData();
        addProductStructureToHtml(data);
    } catch (error) {
        console.error('Error:', error);
    }
})();
