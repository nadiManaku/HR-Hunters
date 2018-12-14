import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { mimeType } from "../../../validators/mime-type.validator";
import { ClientService } from "src/app/services/client.service";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: "app-client-profile",
  templateUrl: "./client-profile.component.html",
  styleUrls: ["./client-profile.component.scss"]
})
export class ClientProfileComponent implements OnInit {
  imagePreview: string | ArrayBuffer;
  imageValid = true;
  loggedInUser;
  validEmail = new RegExp(
    "[a-zA-Z0-9.-_]{1,}@[a-zA-Z.-]{2,}[.]{1}[a-zA-Z]{2,}"
  );
  validPhonenumber = new RegExp(
    "^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$"
  );
  //ex: format: +61 01 2345 6789

  constructor(
    private fb: FormBuilder,
    private clientService: ClientService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.imagePreview =
      "https://about.canva.com/wp-content/uploads/sites/3/2016/08/Band-Logo.png";

    this.loggedInUser = this.authService.getUser();
  }

  clientProfileFormHP = this.fb.group({
    companyName: [
      "",
      Validators.compose([
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(50),
        Validators.pattern("[a-zA-Z0-9]*")
      ])
    ],
    companyEmail: [
      "",
      Validators.compose([
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(30),
        Validators.pattern(this.validEmail)
      ])
    ],
    phonenumber: [
      "",
      Validators.compose([
        Validators.required,
        Validators.pattern(this.validPhonenumber)
      ])
    ],
    location: [
      "",
      Validators.compose([
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(30),
        Validators.pattern("[a-zA-Z0-9]*")
      ])
    ],
    logo: [
      "",
      {
        validators: [Validators.required],
        asyncValidators: [mimeType]
      }
    ]
  });

  buildClientDataOnUpdateClientProfile(
    companyName: string,
    companyEmail: string,
    location: string,
    phoneNumber: string,
    status: string
  ) {
    const newClientData = {
      companyName: companyName,
      companyEmail: companyEmail,
      location: location,
      phoneNumber: phoneNumber,
      status: status
    };
    return newClientData;
  }

  onImagePicked(event: Event) {
    const file = (event.target as HTMLInputElement).files[0];
    const reader = new FileReader();
    reader.onload = () => {
      let img = new Image();
      img.src = reader.result.toString();
      setTimeout(() => {
        if (img.height < 600 || img.width < 600) {
          this.clientProfileFormHP.patchValue({ logo: file });
          this.clientProfileFormHP.controls["logo"].updateValueAndValidity();
          this.imagePreview = reader.result;
          this.imageValid = true;
        } else {
          this.imageValid = false;
        }
      }, 1000);
    };
    reader.readAsDataURL(file);
  }

  onSubmitClientProfile() {
    this.clientProfileFormHP.controls["companyName"].markAsTouched();
    this.clientProfileFormHP.controls["companyEmail"].markAsTouched();
    this.clientProfileFormHP.controls["phonenumber"].markAsTouched();
    this.clientProfileFormHP.controls["location"].markAsTouched();

    let clientData = this.buildClientDataOnUpdateClientProfile(
      this.clientProfileFormHP.value.companyName,
      this.clientProfileFormHP.value.companyEmail,
      this.clientProfileFormHP.value.location,
      this.clientProfileFormHP.value.phonenumber,
      null
    );

    if (this.imagePreview === undefined) {
      this.imageValid = false;
    } else {
      if (this.clientProfileFormHP.valid) {
        this.clientService.updateClientProfile(clientData, this.loggedInUser.id);
      }
    }
  }
}
