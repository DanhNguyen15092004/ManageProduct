import dataFromApi from './data.js';
let  prodName ;
let Price ;
let wrapProd = document.querySelector(".wrapProductItem");
let Date;
let id ;
async function Product() {
    try {
      let data = await dataFromApi() ; 
   
    
        let productElement = document.createElement('li');
        productElement.className = 'ProductItem';
        prodName = data[i].productsName;
        Price = data[i].currentPrice
        Date = data[i].dateCreate
     productElement.innerHTML = await `
                 <ul id="ProdProperty">
                    <li id = "ProdName" class="prodAttribute">Tên sản phẩm : ${prodName}</li>
                    <li id = "ProdPrice" class="prodAttribute">Giá tiền : ${Price}</li>
                    <li id = "DateCreate"  class="prodAttribute">Ngày tạo : ${Date}</li>
                    <i class="fa-solid fa-trash-can deleteButton" onclick="DeleteItem(${data[i].Id})"></i>
                </ul>    
    `;  
           await  wrapProd.appendChild(productElement);
      }
      let deleteButtons = document.querySelectorAll(".deleteButton")
       function DeleteItem(productId){
            deleteButtons.forEach((element)=>{
                element.addEventListener("click",async ()=>{
                await fetch(dataApi,{
                    method:"DELETE",
                    headers:{
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(productId)
                }).then(res =>{
                    if(!res.ok){
                        throw new Error("Network response was not ok");
                    }else {
                        return response.json();
                    }
                }).then((responseData) => {
                    console.log(responseData);
                })
                .catch((error) => {
                console.error("There was a problem with the fetch operation:", error);
                })  
            })
          })
    
      }
    } catch (error) {
        console.log('Error:', error);
      }
};  
   

Product();
