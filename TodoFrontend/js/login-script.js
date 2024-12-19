const form = document.querySelector("#todo-form");

async function postLogin(loginDetails) {
    const url = `http://localhost:5271/login/`;
    try {
        console.log("Sending data to server:", loginDetails);

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
        console.log("Sending data to server:", loginDetails);

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

function getLogInDetails(form) {
    const nameInput = form.elements["Username"].value;
    const passwordInput = form.elements["Password"].value;
    
    return {
        Name: nameInput,
        Password: passwordInput,
    };
}

function handleRegister(form){
    let loginDetails = getLogInDetails(form);
    postRegister(loginDetails);
}

function handleLogin(form){
    let loginDetails = getLogInDetails(form);
    postLogin(loginDetails);
}


// we want a different event listener for each button, add when the dom is made
document.addEventListener("DOMContentLoaded", () => {

    //given these elements of the dom, we register event listeners to the buttons and use the form in the callback
    const loginButton = document.getElementById("login-btn");
    const registerButton = document.getElementById("register-btn");
    const form = document.getElementById("login-form");

    console.log(loginButton)
    console.log(registerButton)
    console.log(handleLogin)
    console.log(handleRegister)

    if (!registerButton || !loginButton) {
        console.error("Login or Register button not found in the DOM.");
        return;
    }

    loginButton.addEventListener("click", (event) =>{
        event.preventDefault();
        handleLogin(form);
    })

    registerButton.addEventListener("click", (event) =>{
        event.preventDefault();
        handleRegister(form);
    })
})
