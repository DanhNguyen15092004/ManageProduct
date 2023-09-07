import fetchData from './data.js';
let  prodName ;
let Price ;
let wrapProd = document.querySelector(".wrapProductItem");
let Date;
let id;

async function addProductStructureToHtml(productList) {
    for (const productAttribute of productList) {
        const { productsName, currentPrice, dateCreate, id } = productAttribute;
        const productElement = document.createElement('li');
        productElement.className = 'ProductItem';

        productElement.innerHTML = `
            <ul id="ProdProperty">
                <li class="prodAttribute">
                Tên sản phẩm : ${productsName}
                </li>
                <li class="prodAttribute">Giá tiền : ${currentPrice} đ
                </li>
                <li class="prodAttribute">Ngày tạo : ${dateCreate}</li>
                <i class="fa-solid fa-trash-can deleteButton" id="${id}"></i>
            </ul>
        `;

        const deleteButton = productElement.querySelector(".deleteButton");
        deleteButton.addEventListener("click", async (event) => {
            if(confirm("Bạn có muốn xóa không")){
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
                        if(confirm("Đã xóa thành công bạn có muốn load lại trang để thấy sự thay đổi hay không")) {
                            location.reload();
                        }
                        
                    }
                } catch (error) {
                    console.error('Error:', error);
                }
            }
          
        });

        wrapProd.appendChild(productElement);
    }
}

(async () => {
    try {
        const data = await fetchData();
        addProductStructureToHtml(data);
    } catch (error) {
        console.error('Error:', error);
    }
})();
