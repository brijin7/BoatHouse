/***************** Get City *********************/

let ddlCity = $('#ddlCity');
let $ddlZone = $('#ddlZone');
let $ddlMMC = $('#ddlMMC');
let $ddlWard = $('#ddlWard');

const getCity = async (EmpId) => {
   
    $ddlZone.empty()
    $ddlMMC.empty()
    $ddlWard.empty()
    //await axios.get('http://192.168.1.220:8081/VCMCService.svc/GetCityData?EmpId='+EmpId+'')
    await axios.get('http://prematix.net/VCMCAPI/VCMCService.svc/GetCityData?EmpId='+EmpId+'')
      .then(async (res) => {
          const {data} = res;
          console.log(data[0]["Response"]);
          const response = data[0]["Response"];
          await _.map(response,v=>{
              let temp = `<option value="${v.CityId}"> ${v.CityName}</option>`;
              console.log(temp)
              ddlCity.append(temp)
          })
          $('#ddlCity').multiselect('rebuild');
      })
            .catch(function(error) {
                console.log(error);
            });
}

/***************** Get Zone By CityID  *********************/


const ZonyByCity = async (EmpId,City) => {   
    $ddlZone.empty()   
   // alert(EmpId,City)
    //await axios.get('http://192.168.1.220:8081/VCMCService.svc/ZoneByCity?EmpId='+EmpId+'&City='+City+'')
    await axios.get('http://prematix.net/VCMCAPI/VCMCService.svc/ZoneByCity?EmpId='+EmpId+'&City='+City+'')
      .then(async (res) => {
     
          console.log(res);
          const {data} = res;
          const response = data[0]["Response"];
          await _.map(response,v=>{
              let temp = `<option value="${v.ZoneId}"> ${v.ZoneName}</option>`;
              console.log(temp)
              $ddlZone.append(temp)
          })
          $ddlZone.multiselect('rebuild');

      })
        .catch(function(error) {
            console.log(error);
            console.log($ddlZone);
        });
}

/***************** Get MCC By City and Zone  *********************/


const MCCByZone = async (EmpId,City, Zone) => {
    $ddlMMC.empty()
    $ddlWard.empty()
   // await axios.get('http://192.168.1.220:8081/VCMCService.svc/MCCByZone?EmpId='+EmpId+'&City='+City+'&Zone='+Zone+'')
    await axios.get('http://prematix.net/VCMCAPI/VCMCService.svc/MCCByZone?EmpId='+EmpId+'&City='+City+'&Zone='+Zone+'')
      .then(async (res) => {
          console.log(res);
         
          const {data} = res;
          const response = data[0]["Response"];
          await _.map(response,v=>{
              let temp = `<option value="${v.MCCId}"> ${v.MCCName}</option>`;
              console.log(temp)
              $ddlMMC.append(temp)
          })
          $ddlMMC.multiselect('rebuild');

      })
        .catch(function(error) {
            console.log(error);
        });
}

/***************** Get Ward By City, Zone and MCC *********************/


const WardByMCC = async (EmpId,City, Zone, MCC) => {
    $ddlWard.empty()
    await axios.get('http://192.168.1.220:8081/VCMCService.svc/WardByMCC?EmpId='+EmpId+'&City='+City+'&Zone='+Zone+'&MCC='+MCC+'')
      .then(async (res) => {
          console.log(res);
        
          const {data} = res;
          const response = data[0]["Response"];
          await _.map(response,v=>{
              let temp = `<option value="${v.WardId}"> ${v.WardName}</option>`;
              console.log(temp)
              $ddlWard.append(temp)
          })
          $ddlWard.multiselect('rebuild');

      })
            .catch(function(error) {
                console.log(error);
            });
}

