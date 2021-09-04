var arr = [];

function chselect(choice_id) 
{
    for (let i = 0; i < arr.length; i++)
    {
        itemsss = arr[i];
        if (itemsss == choice_id)
        {
            alert("Service is already selected.");
            break;
        }
    }

    if (choice_id == 1) 
    {
        document.getElementById("c1").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb1").checked = true;
    }

    if (choice_id == 2)
    {
        document.getElementById("c2").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb2").checked = true;
    }

    if (choice_id == 3)
    {
        document.getElementById("c3").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb3").checked = true;
    }

    if (choice_id == 4)
    {
        document.getElementById("c4").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb4").checked = true;
    }

    if (choice_id == 5) 
    {
        document.getElementById("c5").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb5").checked = true;
    }

    if (choice_id == 6)
    {
        document.getElementById("c6").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb6").checked = true;
    }

    if (choice_id == 7)
    {
        document.getElementById("c7").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb7").checked = true;
    }

    if (choice_id == 8)
    {
        document.getElementById("c8").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb8").checked = true;
    }

    if (choice_id == 9) 
    {
        document.getElementById("c9").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb9").checked = true;
    }
    if (choice_id == 10)
    {
        document.getElementById("c10").style.backgroundColor = "rgba(0, 0, 0, .25)";
        arr.push(choice_id);
        document.getElementById("cb10").checked = true;
    }
}

function clear_select() 
{
    arr.splice(0, arr.length);
    document.getElementById("c1").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c2").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c3").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c4").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c5").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c6").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c7").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c8").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c9").style.backgroundColor = "rgba(250, 250, 250, .25)";
    document.getElementById("c10").style.backgroundColor = "rgba(250, 250, 250, .25)";
}