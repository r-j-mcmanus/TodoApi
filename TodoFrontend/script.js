const form = document.querySelector("#todo-form")

form.addEventListener("submit", (event) =>
{
    event.preventDefault(); // strops the browser automatically doing things, this script is wholey responsible

    //add todo and render

    form.reset(); // puts all input fields back to normal
});