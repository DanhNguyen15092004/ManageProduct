import fetchData from './data.js';
let  prodName ;
let Price ;
let wrapProd = document.querySelector(".wrapProductItem");
let Date;

    async function addProductStructureToHtml(productAttribute){
        let productElement = document.createElement('li');
        productElement.className = 'ProductItem';
        prodName = productAttribute.productsName;
        Price = productAttribute.currentPrice
        Date = productAttribute.dateCreate
     productElement.innerHTML = await 
                `<ul id="ProdProperty">
                    <li id = "ProdName" class="prodAttribute">Tên sản phẩm : ${prodName}</li>
                    <li id = "ProdPrice" class="prodAttribute">Giá tiền : ${Price}</li>
                    <li id = "DateCreate"  class="prodAttribute">Ngày tạo : ${Date}</li>
                    <i class="fa-solid fa-trash-can deleteButton" id="${productAttribute.id}""></i>
                </ul> `
             await  wrapProd.appendChild(productElement);
                    


                   
                    document.addEventListener("click",async (event)=>{
                            let prodID =  event.target.id;
                            console.log(prodID)
                        await fetch(`http://localhost:8080/api/product/${prodID}`,{
                            method:"DELETE",
                            headers:{
                                "Content-Type": "application/json"
                            },
                          
                        }).then(res =>{
                            if(!res.ok){
                                throw new Error("Network response was not ok");
                            }else {
                                console.log("Xóa thành công ");
                            }
                        }).then((responseData) => {
                            console.log(responseData);
                        })
                        .catch((error) => {
                        console.error("There was a problem with the fetch operation:", error);
                        })  
                    })}
                    
            
        (async () => {
            try {
                const data = await fetchData();
                addProductStructureToHtml(data);
            
            } catch (error) {
                console.error('Error:', error);
            }
        })();
   

   

      
    