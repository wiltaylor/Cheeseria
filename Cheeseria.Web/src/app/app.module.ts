import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './views/home/home.component';
import { CheeseComponent } from './views/cheese/cheese.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatGridListModule } from '@angular/material/grid-list'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CheeselistComponent } from './components/cheeselist/cheeselist.component';
import { HttpClientModule } from '@angular/common/http';
import { MatCardModule} from '@angular/material/card';
import { CheeseinfoComponent } from './components/cheeseinfo/cheeseinfo.component';
import { CalculatorComponent } from './components/calculator/calculator.component';
import { MatInputModule } from '@angular/material/input'; 
import { FormsModule } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon'; 

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CheeseComponent,
    CheeselistComponent,
    CheeseinfoComponent,
    CalculatorComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatToolbarModule,
    MatGridListModule,
    MatCardModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatInputModule,
    FormsModule, MatIconModule
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
