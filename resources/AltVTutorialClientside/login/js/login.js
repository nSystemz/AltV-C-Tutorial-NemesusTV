    function WarnungSetzen(text) 
    {
        if(text.length > 0)
        {
            document.getElementById('warning').style.visibility = "visible";
            document.getElementById('warning').innerHTML = text;
        }
    }
    alt.on('ErrorMessage', (txt) => WarnungSetzen(txt));
    function Login() 
    {
        let password = document.getElementById("pass1").value;
	let name = document.getElementById("name").value;

        if(password.length < 6 || name.length < 3) 
        {
            WarnungSetzen("Das eingebene Passwort/der Name ist zu kurz!");
        } 
        else 
        {
	     alt.emit('Auth.Login', name, password);
        }
    }
    function Register() 
    {
        let password = document.getElementById("pass1").value;
	let password2 = document.getElementById("pass2").value;
	let name = document.getElementById("name").value;

        if(password.length < 6 || name.length < 3) 
        {
            WarnungSetzen("Das eingebene Passwort/der Name ist zu kurz!");
        } 
        else 
        {
	    if(password != password2)
	    {
		WarnungSetzen("Die Beiden Passwörter müssen übereinstimmen!");
	    }
            else
	    {
	    	alt.emit('Auth.Register', name, password, password2);
	    }
        }
    }