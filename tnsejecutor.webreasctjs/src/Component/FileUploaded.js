import React, { useState } from "react";
import { saveAs } from "file-saver";
import { BASE_URL } from "../shared";
import axios from "axios";
import { Alert, Button, Form, Card } from "react-bootstrap";
import Spinner from "react-bootstrap/Spinner";
import style from "./index.css";

const FileUploaded = () => {
  const [documentUser, setDocumentUser] = useState("");
  const [file, setFile] = useState(null);
  const [errors, setErrors] = useState([]);
  const [response, setResponse] = useState([]);
  const [loadingApi, setLoadingApi] = useState(false);
  const [showDownloadButton, setShowDownloadButton] = useState(false);

  /**
   * Validacion de errores
   * @param {*} document
   */
  const validate = (document) => {
    const errors = [];
    if (document.length === 0 && errors.length <= 0) {
      errors.push({
        variant: "danger",
        message: "Documento no puede estar vacio",
      });
    }
    if (document.length < 4 && errors.length <= 0) {
      errors.push({
        variant: "danger",
        message: "Documento debe tener almenos 4 caracteres",
      });
    }
    if (document.length > 15 && errors.length <= 0) {
      errors.push({
        variant: "danger",
        message: "Documento no puede ser mayor a 15 caracteres",
      });
    }
    return errors;
  };

  const downLoadFile = () => {
    let txtInformation = "";
    response.map((item) => {
      txtInformation += `${item.name}: ${item.value} \n`;
    });
    var blob = new Blob([txtInformation], {
      type: "text/plain",
    });
    saveAs(blob, "ArchivoSalida.txt");
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const errors = validate(documentUser);
    if (errors.length > 0) {
      setErrors(errors);
      return;
    }
    console.debug(`Document ${documentUser}`);
    sendInformation();
  };

  const sendInformation = async () => {
    setLoadingApi(true);
    setShowDownloadButton(false);
    setErrors([]);
    let formData = new FormData();
    formData.append("file", file);
    formData.append("id", documentUser);

    await axios
      .post(BASE_URL, formData)
      .then((response) => {
        console.log(response);
        if (response.data) {
          setResponse(response.data);
          setLoadingApi(false);
          setShowDownloadButton(true);
        } else {
          setLoadingApi(false);
          setErrors([
            {
              variant: "danger",
              message:
                "Se presento un error al obtener la información de la API",
            },
          ]);
        }
      })
      .catch((error) => {
        console.error(error);
        setLoadingApi(false);
        setErrors([
          {
            variant: "danger",
            message: "Se presento un error al obtener la información de la API",
          },
        ]);
      });
  };

  const setFileInformation = () => {
    var file = document.getElementById("uploadFile").files[0];
    console.log(file);
    var textFile = /text.*/;

    if (file.type.match(textFile)) {
      console.debug("File saved in state");
      setFile(file);
    } else {
      errors.push({
        variant: "danger",
        message: "Debe adjuntar un archivo de texto!",
      });
    }
  };

  return (
    <Card style={{ width: "400px", margin: "0 auto" }}>
      <Card.Header as="h2">Mudanzas</Card.Header>
      <Card.Body style={{ padding: "20px" }}>
        <Form onSubmit={handleSubmit}>

          <Form.Group controlId="formBasicEmail">
            <Form.Label>Documento</Form.Label>
            <Form.Control
              value={documentUser}
              onChange={(evt) => setDocumentUser(evt.target.value)}
              type="number"
              placeholder="Ingresa tu documento"
            />
            <Form.Text className="text-muted">máximo 15 caracteres</Form.Text>
          </Form.Group>


          <Form.Group>
            <Form.File
              type="file"
              onChange={setFileInformation}
              id="uploadFile"
              label="Selecciona un archivo de texto"
            />
          </Form.Group>

          {errors.map((error, idx) => (
            <Alert key={idx} variant={error.variant}>
              {error.message}
            </Alert>
          ))}


          <div className={style.buttonContainer}>
            <Button type="submit">Insertar nuevo Ejecutor</Button>
            {showDownloadButton && (
              <Button
                onClick={() => downLoadFile()}
                style={{ marginLeft: "10px" }}
                variant="secondary"
              >
                Descargar archivo
              </Button>
            )}
          </div>
        </Form>
        
        {loadingApi ? (
          <Spinner
            style={{ marginTop: "10px" }}
            animation="border"
            role="status"
          >
            <span className="sr-only">Cargando...</span>
          </Spinner>
        ) : null}
      </Card.Body>
    </Card>
  );
};

export default FileUploaded;
