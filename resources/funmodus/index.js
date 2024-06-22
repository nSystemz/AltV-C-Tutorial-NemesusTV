let funmodus = false;

alt.onServer('setFunmodus', () => {
    funmodus = !funmodus;
})

alt.on('keydown', (key) => {
    //Funmodus
    let vehicle = lPlayer.vehicle;
    if(funmodus)
    {
        //Jump
        if(key == 17)
        {
            if(vehicle)
            {
                let velo = native.getEntityVelocity(vehicle);
                native.setEntityVelocity(vehicle, velo.x* 2.25, velo.y* 2.25, velo.z);
            }
        }
        //Speedboost
        if(key == 18)
        {
            if(vehicle)
            {
                let velo = native.getEntityVelocity(vehicle);
                native.setEntityVelocity(vehicle, velo.x+ 0.1, velo.y+ 0.1, velo.+ 7.5);
            }
        }
    }
})