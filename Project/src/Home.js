import fetchData from './data.js';
let wrapProd = document.querySelector(".wrapProductItem");

    // Render data
async function addProductStructureToHtml(productList) {
    for (const productAttribute of productList){
        const { productsName, currentPrice, dateCreate, id,prodType } = productAttribute;
        const productElement = document.createElement('li');
        productElement.className = 'ProductItem';
        productElement.innerHTML = 
        `
            <ul id="ProdProperty">
                <li class="prodAttribute">
                Tên sản phẩm : ${productsName}
                <i class="fa-solid fa-pen-to-square edit editName"></i>
                </li>
                <li class="prodAttribute">Giá tiền : ${currentPrice}đ
                <i class="fa-solid fa-pen-to-square edit editPrice"></i>
                </li>
                <li class="prodAttribute">Loại sản phẩm : ${prodType}
                <i class="fa-solid fa-pen-to-square edit editType"></i>
                </li>
                <li class="prodAttribute">Ngày tạo : ${dateCreate}</li>
                <i class="fa-solid fa-trash-can deleteButton" id="${id}"></i>
            </ul>   
        `;

        //delete Method
        const deleteButton = productElement.querySelector(".deleteButton");
        deleteButton.addEventListener("click", async (event) => {
            if(confirm("Xóa là mất hết tất cả sản phẩm !!! Bạn có muốn xóa không")){
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
                            location.reload();
                    }
                } catch (error) {
                    console.error('Error:', error);
                }
            }
        });

            

          
       
        wrapProd.appendChild(productElement);
    }

    
    const productItem = document.querySelectorAll(".ProductItem");
    let editButtonCheck = true;
     function toggle(OpenEditButton){
            productItem.forEach(buttons =>{
                 let childEditButtons =  buttons.querySelectorAll(".edit");

                 childEditButtons.forEach(button =>{
                    button.style.visibility = editButtonCheck ? "visible" : "hidden";
                 })   
            })
                OpenEditButton.innerHTML = editButtonCheck ?   `<i class="fa-solid fa-x closeEditButton"></i> Đóng` : "Chỉnh sửa sản phẩm"
                editButtonCheck = !editButtonCheck;   
        }
        document.querySelector(".editButton").addEventListener("click", (event) => {
              toggle(event.target);        
        });
    }
        


// Passing data to render
(async () => {
    try {
        const data = await fetchData();
        addProductStructureToHtml(data);
    } catch (error) {
        console.error('Error:', error);
    }
})();
