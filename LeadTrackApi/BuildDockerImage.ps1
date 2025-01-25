param(
    [switch]$Deploy
)

## Definir el nombre del repositorio y la imagen
$serviceName = "lead-web-api"
$imageTag = "us-central1-docker.pkg.dev/leadtrack-448703/jam-repo/$serviceName"


try {
    dotnet clean
    Write-Host "Creación de la imagen"
    docker build -t $imageTag . 
    if ($LASTEXITCODE -ne 0) { throw "La construcción falló. No se puede continuar con el push ni el despliegue." }

    if ($Deploy) {
        Write-Host "Empujando la imagen al repositorio..."
        docker push $imageTag 
        if ($LASTEXITCODE -ne 0) { throw "Subida de imagen $serviceName falló." }
        Write-Host "Imagen $serviceName subida correctamente."

        Write-Host "Desplegando $serviceName en Google Cloud Run..."
        gcloud run deploy $serviceName --image $imageTag --platform managed --allow-unauthenticated 
        if ($LASTEXITCODE -ne 0) { throw "El despliegue en Google Cloud Run falló." }
        Write-Host "Despliegue exitoso en Google Cloud Run."
    } else {
        Write-Host "El despliegue no se realizará ya que la opción -d no fue proporcionada."
    }
}
catch {
    Write-Host "Ocurrió un error: $_"
}
