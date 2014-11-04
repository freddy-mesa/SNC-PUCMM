package sncpucmm

class Ubicacion {

    String nombre
    String abreviacion

    static constraints = {
        abreviacion nullable: true
    }
}