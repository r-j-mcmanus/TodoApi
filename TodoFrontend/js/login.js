const form = document.getElementById("login-form");
const loginBtn = document.getElementById("login-btn");
const registerBtn = document.getElementById("register-btn");

form.addEventListener('submit', async (event) => {
    event.preventDefault(); // prevents reloading on click

    const nameInput = form.elements["username"].value;
    const passwordInput = form.elements["password"].value;

    const loginObj = {
        'UserName': nameInput, 
        'Password': passwordInput
    };

    if(event.submitter.id == "login-btn"){
        let response = await postRequest(loginObj, 'login');
        if (!response.ok) {
            if(response.status == 401) {
                console.log('Username or password incorrect')
            }
            const data = response.json();
            console.log('unknown error', data)
        }
        else{
            console.log('Successfully logged in!')
            console.log(await response.json());
        }
    } 
    else if (event.submitter.id == "register-btn"){
        let response = await postRequest(loginObj, 'register');
        
        if (!response.ok) {
            if(response.status == 409) {
                console.log('Username already exists')
            }
            const data = response.json();
            console.log('unknown error', data)
        }
        else{
            console.log('Successfully registered!')
            console.log(await response.json());
        }
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
        return response;
    } catch (error) {
        console.log("there was an error", error)
    }
};
