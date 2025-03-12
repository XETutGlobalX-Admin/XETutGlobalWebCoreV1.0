function labelTemplate(iconName) {
    return (data) => $(`<div><i class='dx-icon dx-icon-${iconName}'></i>${data.text}</div>`);
}
function SubmitHelDesk() {
    debugger;
    // Get only input elements with a 'name' attribute
    let controlid;
    var formInstance = $("#help_otms_deskformSection").dxForm("instance");

    // Add a button click event listener
    // Get the value of a specific form control by its dataField
    let FirstName_controlValue = formInstance.option("formData").FirstName;
    let LastName_controlValue = formInstance.option("formData").LastName;
    let BirthDate_controlValue = formInstance.option("formData").BirthDate;
    let Query_controlValue = formInstance.option("formData").Query;
    let Address_controlValue = formInstance.option("formData").Address;
    let Phone_controlValue = formInstance.option("formData").Phone;
    let Email_controlValue = formInstance.option("formData").Email;
    const helpdesk_formdata = {
       
        FirstName: FirstName_controlValue,
        LastName: LastName_controlValue,

        BirthDate: BirthDate_controlValue,

        Query: Query_controlValue,
        Address: Address_controlValue,
        Phone: Phone_controlValue,
        Email: Email_controlValue,
    };

    console.log("Control Values:", JSON.stringify(helpdesk_formdata));
    HitToAPI("SubMit_Help_Desk_form", JSON.stringify(helpdesk_formdata));
    DevExpress.ui.notify({
        message: 'You have submitted the form',
        position: {
            my: 'center top',
            at: 'center top',
        },
    }, 'success', 500);

    
}

function BindHelpDeskForm() {
    debugger;
    const helpdeskdefault__formdata = {
        ID: 1,
        FirstName: '',
        LastName: '',
        
        BirthDate: '',
        
        Query: '',
        Address: '',
        Phone: '',
        Email: '',
    };
    const form = $('#help_otms_deskformSection').dxForm({
        labelMode: 'floating',
        formData: helpdeskdefault__formdata,
        items: [{
            itemType: 'group',
            caption: 'Inquiry Form',
            colCount: 2,
            items: [{
                dataField: 'FirstName',
                editorOptions: {
                    id: 'txthelpFirstName',
                    disabled: false,
                },
                
            }, {
                dataField: 'LastName',
                editorOptions: {
                    disabled: false,
                },
                
            }, {
                dataField: 'BirthDate',
                editorType: 'dxDateBox',
                editorOptions: {
                    disabled: false,
                    width: '100%',
                },
                
            }, {
                dataField: 'Address',
                
            }, {
                colSpan: 2,
                dataField: 'Query',
                editorType: 'dxTextArea',
                editorOptions: {
                    height: 90,
                    maxLength: 200,
                },
                
            }, {
                dataField: 'Phone',
                
                label: {
                    template: labelTemplate('tel'),
                },
            }, {
                dataField: 'Email',
                
            }],
        },
            {
                itemType: 'group',
                cssClass: 'last-group',
                colCount: 'auto',
                colCountByScreen: {
                    md: 2,
                    sm: 2,
                },
                items: [{
                        itemType: 'button',
                        buttonOptions: {
                            text: 'Register',
                            type: 'default',
                            onClick: () => {
                                SubmitHelDesk();
                            },
                            //width: '120px',
                        },
                    }],
            }],
    }).dxForm('instance');
}