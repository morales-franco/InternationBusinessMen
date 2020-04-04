# **International Business Men**
 
**Business Notes**

*PLAN B*: cuando se consulta a los web services para obtener transacciones o rates al mismo tiempo grabamos las mismas en una base de datos SQL.
En el caso que de exception la llamada a los web services, el sistema logea el error y devuelve el ultimo set de datos que fue persistido en la base de datos.

*Products controller - "products/{productId}/transactions"*: : cuando se invoque este action result el sistema retornara el set de transactions asociadas al producto (SKU)
y la sumatoria total de las mismas. Para esto pasara el valor de todas las transacciones de la moneda original a EUROS y luego realizara la sumatoria de las mismas. 
A su vez utilizara "Round to even" cuando realiza las conversiones de moneda.

**Technical Notes**

Para la realización del sistema se utilizo una Arquitectura multicapas utilizando service pattern and repository pattern.
Se utiliza ASPNET CORE 3.1 (API) + Netstandard libraries.

*InternationalBusinessMen.API* 
Este proyecto es el API que va  a estar escuchando los request para esto expone 3 controllers.
Un controller siempre devuelve DTOs al cliente, nunca entidades de dominio.
Se utiliza automapper para transformar entidades de negocio en dtos que se devuelven al cliente.
Esta capa tiene sus propios services, en este caso estos son UI services. Por ejemplo cuando un DTO se conforma de varias entidades o hay que hacer un agrupamiento o
operaciones de este estilo, se utilizan estos UI Services.
Se utiliza Serilog para loggear las aplicaciones.
Se utiliza net core built-in dependency injection.
Se configura dependency injection registrando los services y repositories para que pueden ser inyectados por constructor.
Se utiliza Swagger para presentar los endpoints expuestos.

*InternationalBusinessMen.CORE*
Aqui se define la logica de negocio de la aplicación.
Se definen entidades de dominio.
Aqui se encuentran la implementación de los servicios, dentro de los mismos estará la lógica de negocio. 

*InternationalBusinessMen.INFRAESTRUCTURE*
Se configura entity framework core --> Se utiliza un localDb
Se implementan los repositorios.
Se implementan External providers para interactuar con Web service externos.

*InternationalBusinessMen.TEST*:
Se realizan algunos test para validar las funcionalidades de la app
