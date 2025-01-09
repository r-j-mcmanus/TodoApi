const form = document.getElementById("login-form");
const loginBtn = document.getElementById("login-btn");
const registerBtn = document.getElementById("register-btn");

form.addEventListener('submit', (event) => {
    event.preventDefault(); // prevents reloading on click

    const nameInput = form.elements["username"].value;
    const passwordInput = form.elements["password"].value;

    const loginObj = {
        'UserName': nameInput, 
        'Password': passwordInput
    };

    if(event.submitter.id == "login-btn"){
        postRequest(loginObj, 'login');
    } 
    else if (event.submitter.id == "register-btn"){
        postRequest(loginObj, 'register');
    }
    form.reset(); // remove filled values form the form
});

async function postRequest(postBody, endpoint) {
    const url = `http://localhost:5271/${endpoint}`
    
    try {
        const response = await fetch(url, 
            {
                method: "POST",
                body: JSON.stringify(postBody),
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        );

        if (!response.ok) {
            if(response.status == 401) {
                console.log('Username or password incorrect')
            }
            throw new Error("Network response was not ok!", response)
        }

        const data = response.json();
        console.log('Successfully updated the server', data)

    } catch (error) {
        console.log("there was an error", error)
    }
};
