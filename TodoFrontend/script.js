const form = document.querySelector("#todo-form")

let TODOs = []

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


    form.reset(); // puts all input fields back to normal
});