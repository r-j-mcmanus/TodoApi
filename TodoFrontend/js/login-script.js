const form = document.querySelector("#todo-form");


async function postLogin(loginDetails) {
    const url = `http://localhost:5271/login/`;
    try {
        console.log("Sending data to server:");

        // Make the PUT request to the server
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(loginDetails)
        });
  
        if (!response.ok) {
            console.error('Server response not OK:', response.status, response.statusText);
            const errorDetails = await response.text(); // Try to capture server error details
            throw new Error(`HTTP error! Status: ${response.status}. Details: ${errorDetails}`);
        }
    
        // Parse the response data (if response is JSON)
        const data = await response.json();
        console.log('Successfully logged in:', data);
        
    } catch (error) {
        console.error('There was an error with postLogin:', error);
    }
}


async function postRegister(loginDetails) {
    const url = `http://localhost:5271/register/`;
    try {
        console.log("Sending data to server:");

        // Make the PUT request to the server
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(loginDetails)
        });
  
        if (!response.ok) {
            console.error('Server response not OK:', response.status, response.statusText);
            const errorDetails = await response.text(); // Try to capture server error details
            throw new Error(`HTTP error! Status: ${response.status}. Details: ${errorDetails}`);
        }
    
        // Parse the response data (if response is JSON)
        const data = await response.json();
        console.log('Successfully logged in:', data);
        
    } catch (error) {
        console.error('There was an error with postLogin:', error);
    }
}

function getLogInDetails() {
    const nameInput = form.elements["Username"].value;
    const passwordInput = form.elements["Password"].value;
    
    return {
        Name: nameInput,
        Password: passwordInput,
    };
}


// we want a different event listener for each button, add when the dom is made
document.addEventListener("DOMContentLoaded", () => {
    //get the elements of the html we want
    const loginButton = document.getElementById("login-btn");
    const registerButton = document.getElementById("register-form");

    async function handleRegister(event){
        let loginDetails = getLogInDetails();
        await postRegister(loginDetails);
    }

    async function handleLogin(event){
        let loginDetails = getLogInDetails();
        await postLogin(loginDetails);
    }

    //given these elements of the dom, we register event listeners to the buttons and use the form in the callback
    loginButton.addEventListener("click", (event) =>{
        event.preventDefault();
        handleLogin(event);
    })

    registerButton.addEventListener("click", (event) =>{
        event.preventDefault();
        handleRegister(event);
    })
})
