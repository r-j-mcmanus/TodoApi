const form = document.getElementById("login-form");

jwt = "";

if (localStorage["jwt"] !== null && 
    localStorage["jwt"] !== undefined) {
    jwt = localStorage["jwt"];
}

form.addEventListener('submit', async (event) => {
    event.preventDefault(); // prevents reloading on click

    const nameInput = form.elements["username"].value;
    const passwordInput = form.elements["password"].value;

    const loginObj = {
        'UserName': nameInput, 
        'Password': passwordInput
    };

    if(event.submitter.id == "login-btn"){
        let data = await login(loginObj);
        localStorage["jwt"] = data['token']
    } 
    else if (event.submitter.id == "register-btn"){
        await register(loginObj);
    }
    else if (event.submitter.id == "test-jwt-btn"){
        let request = await postRequest(localStorage["jwt"], "ValidateJWT", {"Authorization": `Bearer ${localStorage["jwt"]}`});
        console.log(request)
    }
    form.reset(); // remove filled values form the form
});

async function login(loginObj){
    let response = await postRequest(loginObj, 'login');
    if (!response.ok) {
        if(response.status == 401) {
            console.log('Username or password incorrect');
        }
    }
    else{
        console.log('Successfully logged in!');
        const data = await response.json();
        return data;
    }
}

async function register(loginObj){        
    let response = await postRequest(loginObj, 'register');    
    if (!response.ok) {
        if(response.status == 409) {
            console.log('Username already exists');
        }
    }
    else{
        console.log('Successfully registered!');
        const data = await response.json();
        return data;
    }
}

async function postRequest(postBody, endpoint, headers = {}) {
    const url = `http://localhost:5271/${endpoint}`;
    headers['Content-Type'] = 'application/json'

    try {
        const response = await fetch(url, 
            {
                method: "POST",
                body: JSON.stringify(postBody),
                headers: headers
            }
        );
        return response;
    } catch (error) {
        console.log("there was an error", error);
    }
};
