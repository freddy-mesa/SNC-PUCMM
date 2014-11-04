package sncpucmm

class Nodo {

    Ubicacion ubicacion
    Integer edificio
    String nombre
    String activo

    static constraints = {
        ubicacion nullable: true
        edificio nullable: true
    }
}
