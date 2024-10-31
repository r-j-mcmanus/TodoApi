const form = document.querySelector("#todo-form")
const list = document.querySelector("#todo-list")

let TODOs = []

if (localStorage["data"] !== null && 
        localStorage["data"] !== undefined) {
    TODOs = JSON.parse(localStorage["data"])
}

function buildUI(){
    let HTML = ``
    TODOs.forEach((todo) => {
        HTML += `
            <li id="${todo.id}">
                <span> 
                    ${todo.title}
                </span>
                <button class="button-complete">
                    +
                </button>
            </li>
        `
    })
    list.innerHTML = HTML
}

form.addEventListener("submit", (event) =>
{
    event.preventDefault(); // strops the browser automatically doing things, this script is wholey responsible

    //add todo and render
    TODOs.push(
        {
            title: event.target[0].value,
            complete: false,
            id: self.crypto.randomUUID() // after generating 1 billion UUIDs every second for approximately 100 years would the probability of creating a single duplicate reach 50%.
        }
    );

    buildUI() // update UI

    localStorage["data"] = JSON.stringify(TODOs);

    form.reset(); // puts all input fields back to normal
});

buildUI() // initial call UI