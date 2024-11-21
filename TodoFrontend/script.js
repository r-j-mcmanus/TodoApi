const form = document.querySelector("#todo-form");
const list = document.querySelector("#todo-list");

let TODOs = [];

if (localStorage["data"] !== null && 
        localStorage["data"] !== undefined) {
    TODOs = JSON.parse(localStorage["data"]);
}

function buildUI(){
    let HTML = ``;
    TODOs.forEach((todo) => {
        HTML += `
            <li id="${todo.id}">
                <span> 
                    ${todo.name}
                </span>
                <button class="button-complete">
                    +
                </button>
            </li>
        `
    });
    list.innerHTML = HTML;
}


function putTodo(todo) {
    
    // URL for the POST request
    const url = `http://localhost:5271/todoitems`;
  
    try {
        console.log(JSON.stringify(todo));

      // Make the PUT request to the server
      const response = fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(todo) // Send as JSON payload
      });
  
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
  
      // Parse the response data (if response is JSON)
      const data = response.json();
      console.log('Successfully updated on the server:', data);
    } catch (error) {
      // Handle any errors that occur during the fetch
      console.error('There was an error with the PUT request:', error);
    }
}

form.addEventListener("submit", (event) =>
{
    event.preventDefault(); // strops the browser automatically doing things, this script is wholey responsible

    let todo = {
        name: event.target[0].value,
        isComplete: false,
        ref: self.crypto.randomUUID()  // after generating 1 billion UUIDs every second for approximately 100 years would the probability of creating a single duplicate reach 50%.
    }

    //add todo and render
    TODOs.push(todo);

    putTodo(todo)

    buildUI(); // update UI

    localStorage["data"] = JSON.stringify(TODOs);

    form.reset(); // puts all input fields back to normal
});


   

document.documentElement.addEventListener("click", (event) => {
    if(event.target.classList.contains("button-complete")) {
        // remove from todos the list item with the same unique id corresponding to the button
        TODOs = TODOs.filter((todo) => todo.id !== event.target.parentElement.id);
        localStorage["data"] = JSON.stringify(TODOs);
        buildUI(); // update UI
    }
})

buildUI() // initial call UI